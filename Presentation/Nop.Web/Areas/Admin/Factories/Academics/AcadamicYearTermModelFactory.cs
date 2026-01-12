using Nop.Core.Domain.AcademicYears;
using Nop.Services.AcademicYears;
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

    #endregion

    #region Ctor

    public AcademicYearTermModelFactory(
        ILocalizationService localizationService,
        ILocalizedModelFactory localizedModelFactory,
        IStoreMappingSupportedModelFactory storeMappingSupportedModelFactory,
        IAcademicYearService academicYearService
        )
    {
        _localizationService = localizationService;
        _localizationService = localizationService;
        _localizedModelFactory = localizedModelFactory;
        _storeMappingSupportedModelFactory = storeMappingSupportedModelFactory;
        _academicYearService = academicYearService;
    }

    #endregion

    #region Utilities

    

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

        //prepare list model
        var model = await new AcademicYearTermListModel().PrepareToGridAsync(searchModel, academicYearTerms, () =>
        {
            //fill in model values from the entity
            return academicYearTerms.SelectAwait(async academicYearTerm =>
            {
                var academicYearTermModel = academicYearTerm.ToModel<AcademicYearTermModel>();

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


//{{PrepareStoreCode}}

        return model;
    }

    #endregion
}