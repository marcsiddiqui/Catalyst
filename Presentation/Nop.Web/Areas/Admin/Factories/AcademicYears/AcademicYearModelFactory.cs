using Nop.Core.Domain.AcademicYears;
using Nop.Services.AcademicYears;
using Nop.Services.Localization;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.AcademicYears;
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

    #endregion

    #region Ctor

    public AcademicYearModelFactory(IAcademicYearService academicYearService,
        ILocalizationService localizationService,
        ILocalizedModelFactory localizedModelFactory,
        IStoreMappingSupportedModelFactory storeMappingSupportedModelFactory)
    {
        _academicYearService = academicYearService;
        _localizationService = localizationService;
        _localizedModelFactory = localizedModelFactory;
        _storeMappingSupportedModelFactory = storeMappingSupportedModelFactory;
    }

    #endregion

    #region Utilities

    

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
        var academicYears = (await _academicYearService.GetAllAcademicYearsAsync()).ToPagedList(searchModel);

        //prepare list model
        var model = await new AcademicYearListModel().PrepareToGridAsync(searchModel, academicYears, () =>
        {
            //fill in model values from the entity
            return academicYears.SelectAwait(async academicYear =>
            {
                var academicYearModel = academicYear.ToModel<AcademicYearModel>();

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
        }

        //set default values for the new model
        if (academicYear == null)
        {
            
        }

        //prepare localized models
        if (!excludeProperties)
            model.Locales = await _localizedModelFactory.PrepareLocalizedModelsAsync(localizedModelConfiguration);


//{{PrepareStoreCode}}

        return model;
    }

    #endregion
}