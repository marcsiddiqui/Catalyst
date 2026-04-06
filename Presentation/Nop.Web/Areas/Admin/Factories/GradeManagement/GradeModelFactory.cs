using Nop.Core.Domain.GradeManagement;
using Nop.Services.GradeManagement;
using Nop.Services.Localization;
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

    #endregion

    #region Ctor

    public GradeModelFactory(IGradeService gradeService,
        ILocalizationService localizationService,
        ILocalizedModelFactory localizedModelFactory,
        IStoreMappingSupportedModelFactory storeMappingSupportedModelFactory)
    {
        _gradeService = gradeService;
        _localizationService = localizationService;
        _localizedModelFactory = localizedModelFactory;
        _storeMappingSupportedModelFactory = storeMappingSupportedModelFactory;
    }

    #endregion

    #region Utilities

    

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
}