using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core.Domain.AcademicYears;
using Nop.Core.Domain.GradeManagement;
using Nop.Services.AcademicYears;
using Nop.Services.GradeManagement;
using Nop.Services.Localization;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.AcademicYears;
using Nop.Web.Framework.Factories;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Web.Areas.Admin.Factories;

public partial class AcademicYearTermModelFactory : IAcademicYearTermModelFactory
{
    #region Fields

    protected readonly ILocalizationService _localizationService;
    protected readonly ILocalizedModelFactory _localizedModelFactory;
    protected readonly IStoreMappingSupportedModelFactory _storeMappingSupportedModelFactory;
    protected readonly IAcademicYearService _academicYearService;
    protected readonly IGradeService _gradeService;
    protected readonly ISectionService _sectionService;

    #endregion

    #region Ctor

    public AcademicYearTermModelFactory(
        ILocalizationService localizationService,
        ILocalizedModelFactory localizedModelFactory,
        IStoreMappingSupportedModelFactory storeMappingSupportedModelFactory,
        IAcademicYearService academicYearService,
        IGradeService gradeService,
        ISectionService sectionService
        )
    {
        _localizationService = localizationService;
        _localizedModelFactory = localizedModelFactory;
        _storeMappingSupportedModelFactory = storeMappingSupportedModelFactory;
        _academicYearService = academicYearService;
        _gradeService = gradeService;
        _sectionService = sectionService;
    }

    #endregion

    #region Utilities

    public virtual async Task<IDictionary<int, string>> PrepareAcademicYearGradeSectionMappingNamesAsync(IEnumerable<AcademicYearGradeSectionMapping> mappings)
    {
        var mappingList = mappings?.ToList() ?? new List<AcademicYearGradeSectionMapping>();
        if (!mappingList.Any())
            return new Dictionary<int, string>();

        var academicYears = (await _academicYearService.GetAcademicYearsByIdsAsync(mappingList.Select(mapping => mapping.AcademicYearId).Distinct()))?.ToList()
            ?? new List<AcademicYear>();
        var grades = (await _gradeService.GetGradesByIdsAsync(mappingList.Select(mapping => mapping.GradeId).Distinct()))?.ToList()
            ?? new List<Grade>();
        var sections = (await _sectionService.GetSectionsByIdsAsync(mappingList.Select(mapping => mapping.SectionId).Where(sectionId => sectionId > 0).Distinct()))?.ToList()
            ?? new List<Section>();

        return mappingList.ToDictionary(mapping => mapping.Id, mapping =>
        {
            var academicYear = academicYears.FirstOrDefault(year => year.Id == mapping.AcademicYearId);
            var grade = grades.FirstOrDefault(item => item.Id == mapping.GradeId);
            var section = sections.FirstOrDefault(item => item.Id == mapping.SectionId);

            var yearName = academicYear != null
                ? $"{academicYear.StartDate:yyyy} - {academicYear.EndDate:yyyy}"
                : string.Empty;

            return string.Join(" - ", new[] { grade?.Name, section?.Name, yearName }.Where(value => !string.IsNullOrWhiteSpace(value)));
        });
    }

    public virtual async Task PrepareAcademicYearGradeSectionMappingsAsync(IList<SelectListItem> items, int selectedMappingId = 0)
    {
        ArgumentNullException.ThrowIfNull(items);

        items.Add(new SelectListItem
        {
            Text = await _localizationService.GetResourceAsync("Admin.Common.Select"),
            Value = "0",
            Selected = selectedMappingId == 0
        });

        var mappings = (await _academicYearService.GetAllAcademicYearGradeSectionMappingsAsync()).ToList();
        var mappingNames = await PrepareAcademicYearGradeSectionMappingNamesAsync(mappings);

        foreach (var mapping in mappings.OrderBy(mapping => mappingNames.TryGetValue(mapping.Id, out var name) ? name : string.Empty))
        {
            items.Add(new SelectListItem
            {
                Text = mappingNames.TryGetValue(mapping.Id, out var name) ? name : mapping.Id.ToString(),
                Value = mapping.Id.ToString(),
                Selected = mapping.Id == selectedMappingId
            });
        }
    }

    #endregion

    #region Methods

    public virtual Task<AcademicYearTermSearchModel> PrepareAcademicYearTermSearchModelAsync(AcademicYearTermSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        //prepare page parameters
        searchModel.SetGridPageSize();

        return Task.FromResult(searchModel);
    }

    public virtual async Task<AcademicYearTermListModel> PrepareAcademicYearTermListModelAsync(AcademicYearTermSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        //get academicYearTerms
        var academicYearTerms = (await _academicYearService.GetAllAcademicYearTermsAsync()).ToPagedList(searchModel);
        var mappingIds = academicYearTerms.Select(term => term.AcademicYearGradeSectionMappingId).Where(id => id > 0).Distinct().ToList();
        IList<AcademicYearGradeSectionMapping> mappings = mappingIds.Any()
            ? await _academicYearService.GetAllAcademicYearGradeSectionMappingsAsync(ids: mappingIds)
            : new List<AcademicYearGradeSectionMapping>();
        var mappingNames = await PrepareAcademicYearGradeSectionMappingNamesAsync(mappings);

        //prepare list model
        var model = await new AcademicYearTermListModel().PrepareToGridAsync(searchModel, academicYearTerms, () =>
        {
            //fill in model values from the entity
            return academicYearTerms.SelectAwait(async academicYearTerm =>
            {
                var academicYearTermModel = academicYearTerm.ToModel<AcademicYearTermModel>();
                academicYearTermModel.AcademicYearGradeSectionMappingName = mappingNames.TryGetValue(academicYearTerm.AcademicYearGradeSectionMappingId, out var mappingName)
                    ? mappingName
                    : academicYearTerm.AcademicYearGradeSectionMappingId.ToString();

                return academicYearTermModel;
            });
        });

        return model;
    }

    public virtual async Task<AcademicYearTermModel> PrepareAcademicYearTermModelAsync(AcademicYearTermModel model, AcademicYearTerm academicYearTerm, bool excludeProperties = false)
    {
        Func<AcademicYearTermLocalizedModel, int, Task> localizedModelConfiguration = null;

        if (academicYearTerm != null)
        {
            //fill in model values from the entity
            if (model == null)
            {
                model = academicYearTerm.ToModel<AcademicYearTermModel>();
            }

            //define localized model configuration action
            localizedModelConfiguration = async (locale, languageId) =>
            {
                locale.Name = await _localizationService.GetLocalizedAsync(academicYearTerm, entity => entity.Name, languageId, false, false);


            };
        }

        //set default values for the new model
        if (academicYearTerm == null)
        {
            
        }

        //prepare localized models
        if (!excludeProperties)
            model.Locales = await _localizedModelFactory.PrepareLocalizedModelsAsync(localizedModelConfiguration);

        await PrepareAcademicYearGradeSectionMappingsAsync(model.AvailableAcademicYearGradeSectionMappings, model.AcademicYearGradeSectionMappingId);

//{{PrepareStoreCode}}

        return model;
    }

    #endregion
}
