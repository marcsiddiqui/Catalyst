using Microsoft.AspNetCore.Mvc.Rendering;

using Nop.Core.Domain.AcademicYears;
using Nop.Core.Domain.GradeManagement;
using Nop.Services;
using Nop.Services.AcademicYears;
using Nop.Services.GradeManagement;
using Nop.Services.Localization;
using Nop.Services.Subjects;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.AcademicYears;
using Nop.Web.Areas.Admin.Models.GradeManagement;
using Nop.Web.Framework.Factories;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Web.Areas.Admin.Factories;

public partial class AcademicYearModelFactory : IAcademicYearModelFactory
{
    #region Fields

    protected readonly IAcademicYearService _academicYearService;
    protected readonly ILocalizationService _localizationService;
    protected readonly ILocalizedModelFactory _localizedModelFactory;
    protected readonly IStoreMappingSupportedModelFactory _storeMappingSupportedModelFactory;
    protected readonly IGradeService _gradeService;
    protected readonly ISubjectService _subjectService;
    protected readonly ISectionService _sectionService;

    #endregion

    #region Ctor

    public AcademicYearModelFactory(IAcademicYearService academicYearService,
        ILocalizationService localizationService,
        ILocalizedModelFactory localizedModelFactory,
        IStoreMappingSupportedModelFactory storeMappingSupportedModelFactory,
        IGradeService gradeService,
        ISubjectService subjectService,
        ISectionService sectionService
        )
    {
        _academicYearService = academicYearService;
        _localizationService = localizationService;
        _localizedModelFactory = localizedModelFactory;
        _storeMappingSupportedModelFactory = storeMappingSupportedModelFactory;
        _gradeService = gradeService;
        _subjectService = subjectService;
        _sectionService = sectionService;
    }

    #endregion

    #region Utilities

    public virtual async Task PrepareSections(IList<SelectListItem> items, int sectionId = 0)
    {
        ArgumentNullException.ThrowIfNull(items);

        var sections = await _sectionService.GetAllSectionsAsync();
        var selectItems = sections.ToSelectList(s => (s as Section)?.Name);
        foreach (var item in selectItems)
            items.Add(item);
    }

    public virtual async Task PrepareGrades(IList<SelectListItem> items, int subjectId = 0)
    {
        ArgumentNullException.ThrowIfNull(items);

        var grades = await _gradeService.GetAllGradesAsync();
        var selectItems = grades.ToSelectList(s => (s as Grade)?.Name);
        foreach (var item in selectItems)
            items.Add(item);
    }

    #endregion

    #region Methods

    public virtual Task<AcademicYearSearchModel> PrepareAcademicYearSearchModelAsync(AcademicYearSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        //prepare page parameters
        searchModel.SetGridPageSize();

        return Task.FromResult(searchModel);
    }

    public virtual async Task<AcademicYearListModel> PrepareAcademicYearListModelAsync(AcademicYearSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        //get academicYears
        var academicYears = await _academicYearService.GetAllAcademicYearsAsync(
            pageIndex: searchModel.Page - 1,
            pageSize: searchModel.PageSize);

        //prepare list model
        var model = await new AcademicYearListModel().PrepareToGridAsync(searchModel, academicYears, () =>
        {
            //fill in model values from the entity
            return academicYears.SelectAwait(async academicYear =>
            {
                var academicYearModel = academicYear.ToModel<AcademicYearModel>();

                academicYearModel.FormattedStartDate = academicYear.StartDate.ToLocalTime().ToString("dd-MMMM-yyyy");
                academicYearModel.FormattedEndDate = academicYear.EndDate.ToLocalTime().ToString("dd-MMMM-yyyy");

                return academicYearModel;
            });
        });

        return model;
    }

    public virtual async Task<AcademicYearModel> PrepareAcademicYearModelAsync(AcademicYearModel model, AcademicYear academicYear, bool excludeProperties = false)
    {
        Func<AcademicYearLocalizedModel, int, Task> localizedModelConfiguration = null;

        if (academicYear != null)
        {
            //fill in model values from the entity
            if (model == null)
            {
                model = academicYear.ToModel<AcademicYearModel>();
            }

            //define localized model configuration action
            localizedModelConfiguration = async (locale, languageId) =>
            {
                locale.Name = await _localizationService.GetLocalizedAsync(academicYear, entity => entity.Name, languageId, false, false);
            };

            model.StartDate = academicYear.StartDate.ToLocalTime();
            model.EndDate = academicYear.EndDate.ToLocalTime();
        }

        //set default values for the new model
        if (academicYear == null)
        {
            var currentYearAacademicYear = (await _academicYearService.GetAllAcademicYearsAsync(year: DateTime.Now.Year)).FirstOrDefault();
            if (currentYearAacademicYear == null)
            {
                model.StartDate = new DateTime(DateTime.UtcNow.Year, 04, 01);
                model.EndDate = new DateTime(DateTime.UtcNow.Year + 1, 03, 31);
            }
            else
            {
                model.StartDate = new DateTime(DateTime.UtcNow.Year + 1, 04, 01);
                model.EndDate = new DateTime(DateTime.UtcNow.Year + 2, 03, 31);
            }
        }

        //prepare localized models
        if (!excludeProperties)
            model.Locales = await _localizedModelFactory.PrepareLocalizedModelsAsync(localizedModelConfiguration);


//{{PrepareStoreCode}}

        return model;
    }

    #region AcademicYearGradeSectionMapping

    public virtual async Task<AcademicYearGradeSectionMappingListModel> PrepareAcademicYearGradeSectionMappingListModelAsync(AcademicYearGradeSectionMappingSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        //get grades
        var academicYearGradeSectionMappings = (await _academicYearService.GetAllAcademicYearGradeSectionMappingsAsync()).ToPagedList(searchModel);
        var sections = (await _sectionService.GetAllSectionsAsync(ids: academicYearGradeSectionMappings.Select(gsm => gsm.SectionId))).ToList();
        var grades = (await _gradeService.GetAllGradesAsync(ids: academicYearGradeSectionMappings.Select(gsm => gsm.GradeId))).ToList();

        //prepare list model
        var model = await new AcademicYearGradeSectionMappingListModel().PrepareToGridAsync(searchModel, academicYearGradeSectionMappings, () =>
        {
            //fill in model values from the entity
            return academicYearGradeSectionMappings.SelectAwait(async academicYearGradeSectionMapping =>
            {
                var academicYearGradeSectionMappingModel = academicYearGradeSectionMapping.ToModel<AcademicYearGradeSectionMappingModel>();
                var grade = grades.FirstOrDefault(s => s.Id == academicYearGradeSectionMapping.GradeId);
                academicYearGradeSectionMappingModel.GradeName = grade?.Name;
                var section = sections.FirstOrDefault(s => s.Id == academicYearGradeSectionMapping.SectionId);
                academicYearGradeSectionMappingModel.SectionName = section?.Name;

                return academicYearGradeSectionMappingModel;
            });
        });

        return model;
    }

    public virtual async Task<AcademicYearGradeSectionMappingModel> PrepareAcademicYearGradeSectionMappingModelAsync(AcademicYearGradeSectionMappingModel model, AcademicYearGradeSectionMapping grade, bool excludeProperties = false)
    {
        if (grade != null)
        {
            //fill in model values from the entity
            if (model == null)
            {
                model = grade.ToModel<AcademicYearGradeSectionMappingModel>();
            }
        }

        //set default values for the new model
        if (grade == null)
        {

        }

        await PrepareGrades(model.AvailableGrades, model.GradeId);
        await PrepareSections(model.AvailableSections, model.SectionId);

        return model;
    }

    #endregion

    #endregion
}