using Nop.Core.Domain.AcademicYears;
using Nop.Services.AcademicYears;
using Nop.Services.Localization;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.AcademicYears;
using Nop.Web.Framework.Factories;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Web.Areas.Admin.Factories;

public partial class AcadamicYearTermModelFactory : IAcadamicYearTermModelFactory
{
    #region Fields

    protected readonly ILocalizationService _localizationService;
    protected readonly ILocalizedModelFactory _localizedModelFactory;
    protected readonly IStoreMappingSupportedModelFactory _storeMappingSupportedModelFactory;
    protected readonly IAcademicYearService _acadamicYearService;

    #endregion

    #region Ctor

    public AcadamicYearTermModelFactory(
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
        _acadamicYearService = academicYearService;
    }

    #endregion

    #region Utilities

    

    #endregion

    #region Methods

    public virtual Task<AcadamicYearTermSearchModel> PrepareAcadamicYearTermSearchModelAsync(AcadamicYearTermSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        //prepare page parameters
        searchModel.SetGridPageSize();

        return Task.FromResult(searchModel);
    }

    public virtual async Task<AcadamicYearTermListModel> PrepareAcadamicYearTermListModelAsync(AcadamicYearTermSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        //get acadamicYearTerms
        var acadamicYearTerms = (await _acadamicYearService.GetAllAcadamicYearTermsAsync()).ToPagedList(searchModel);

        //prepare list model
        var model = await new AcadamicYearTermListModel().PrepareToGridAsync(searchModel, acadamicYearTerms, () =>
        {
            //fill in model values from the entity
            return acadamicYearTerms.SelectAwait(async acadamicYearTerm =>
            {
                var acadamicYearTermModel = acadamicYearTerm.ToModel<AcadamicYearTermModel>();

                return acadamicYearTermModel;
            });
        });

        return model;
    }

    public virtual async Task<AcadamicYearTermModel> PrepareAcadamicYearTermModelAsync(AcadamicYearTermModel model, AcadamicYearTerm acadamicYearTerm, bool excludeProperties = false)
    {
        Func<AcadamicYearTermLocalizedModel, int, Task> localizedModelConfiguration = null;

        if (acadamicYearTerm != null)
        {
            //fill in model values from the entity
            if (model == null)
            {
                model = acadamicYearTerm.ToModel<AcadamicYearTermModel>();
            }

            //define localized model configuration action
            localizedModelConfiguration = async (locale, languageId) =>
            {
                locale.Name = await _localizationService.GetLocalizedAsync(acadamicYearTerm, entity => entity.Name, languageId, false, false);


            };
        }

        //set default values for the new model
        if (acadamicYearTerm == null)
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