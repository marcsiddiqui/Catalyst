using Microsoft.AspNetCore.Mvc.Rendering;

using Nop.Core.Domain.GradeManagement;
using Nop.Core.Domain.Subjects;
using Nop.Services;
using Nop.Services.GradeManagement;
using Nop.Services.Localization;
using Nop.Services.Subjects;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.GradeManagement;
using Nop.Web.Framework.Factories;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Web.Areas.Admin.Factories;

public partial class GradeModelFactory : IGradeModelFactory
{
    #region Fields

    protected readonly IGradeService _gradeService;
    protected readonly ILocalizationService _localizationService;
    protected readonly ILocalizedModelFactory _localizedModelFactory;
    protected readonly IStoreMappingSupportedModelFactory _storeMappingSupportedModelFactory;
    protected readonly ISubjectService _subjectService;
    protected readonly ISectionService _sectionService;

    #endregion

    #region Ctor

    public GradeModelFactory(IGradeService gradeService,
        ILocalizationService localizationService,
        ILocalizedModelFactory localizedModelFactory,
        IStoreMappingSupportedModelFactory storeMappingSupportedModelFactory,
        ISubjectService subjectService,
        ISectionService sectionService)
    {
        _gradeService = gradeService;
        _localizationService = localizationService;
        _localizedModelFactory = localizedModelFactory;
        _storeMappingSupportedModelFactory = storeMappingSupportedModelFactory;
        _subjectService = subjectService;
        _sectionService = sectionService;
    }

    #endregion

    #region Utilities

    public virtual async Task PrepareSections(IList<SelectListItem> items, int sectionId = 0)
    {
        ArgumentNullException.ThrowIfNull(items);

        var sections = await _sectionService.GetAllSectionsAsync();
        items = [.. sections.ToSelectList(s => (s as Section)?.Name)];
    }

    public virtual async Task PrepareSubjects(IList<SelectListItem> items, int subjectId = 0)
    {
        ArgumentNullException.ThrowIfNull(items);

        var subjects = await _subjectService.GetAllSubjectsAsync();
        items = [.. subjects.ToSelectList(s => (s as Subject)?.Name)];
    }

    #endregion

    #region Methods

    public virtual Task<GradeSearchModel> PrepareGradeSearchModelAsync(GradeSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        //prepare page parameters
        searchModel.SetGridPageSize();

        return Task.FromResult(searchModel);
    }

    public virtual async Task<GradeListModel> PrepareGradeListModelAsync(GradeSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        //get grades
        var grades = (await _gradeService.GetAllGradesAsync()).ToPagedList(searchModel);

        //prepare list model
        var model = await new GradeListModel().PrepareToGridAsync(searchModel, grades, () =>
        {
            //fill in model values from the entity
            return grades.SelectAwait(async grade =>
            {
                var gradeModel = grade.ToModel<GradeModel>();

                return gradeModel;
            });
        });

        return model;
    }

    public virtual async Task<GradeModel> PrepareGradeModelAsync(GradeModel model, Grade grade, bool excludeProperties = false)
    {
        Func<GradeLocalizedModel, int, Task> localizedModelConfiguration = null;

        if (grade != null)
        {
            //fill in model values from the entity
            if (model == null)
            {
                model = grade.ToModel<GradeModel>();
            }

            //define localized model configuration action
            localizedModelConfiguration = async (locale, languageId) =>
            {
                locale.Name = await _localizationService.GetLocalizedAsync(grade, entity => entity.Name, languageId, false, false);


            };
        }

        //set default values for the new model
        if (grade == null)
        {

        }

        //prepare localized models
        if (!excludeProperties)
            model.Locales = await _localizedModelFactory.PrepareLocalizedModelsAsync(localizedModelConfiguration);


        //prepare available stores
        await _storeMappingSupportedModelFactory.PrepareModelStoresAsync(model, grade, excludeProperties);


        return model;
    }

    #endregion

    #region GradeSubjectMapping

    public virtual async Task<GradeSubjectListModel> PrepareGradeSubjectListModelAsync(GradeSubjectSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        //get grades
        var gradeSubjectMappings = (await _gradeService.GetAllGradeSubjectMappingsAsync()).ToPagedList(searchModel);
        var sujects = (await _subjectService.GetAllSubjectsAsync(ids: gradeSubjectMappings.Select(gsm => gsm.SubjectId))).ToList();
        var sections = (await _sectionService.GetAllSectionsAsync(ids: gradeSubjectMappings.Select(gsm => gsm.SectionId ?? 0))).ToList();

        //prepare list model
        var model = await new GradeSubjectListModel().PrepareToGridAsync(searchModel, gradeSubjectMappings, () =>
        {
            //fill in model values from the entity
            return gradeSubjectMappings.SelectAwait(async grade =>
            {
                var gradeModel = grade.ToModel<GradeSubjectMappingModel>();
                var subject = sujects.FirstOrDefault(s => s.Id == grade.SubjectId);
                gradeModel.SubjectName = subject?.Name;
                var section = sections.FirstOrDefault(s => s.Id == grade.SectionId);
                gradeModel.SectionName = section?.Name;

                return gradeModel;
            });
        });

        return model;
    }

    public virtual async Task<GradeSubjectMappingModel> PrepareGradeSubjectMappingModelAsync(GradeSubjectMappingModel model, GradeSubjectMapping grade, bool excludeProperties = false)
    {
        if (grade != null)
        {
            //fill in model values from the entity
            if (model == null)
            {
                model = grade.ToModel<GradeSubjectMappingModel>();
            }
        }

        //set default values for the new model
        if (grade == null)
        {

        }

        await PrepareSections(model.AvailableSections, model.SectionId ?? 0);
        await PrepareSubjects(model.AvailableSubjects, model.SubjectId);

        return model;
    }

    #endregion
}