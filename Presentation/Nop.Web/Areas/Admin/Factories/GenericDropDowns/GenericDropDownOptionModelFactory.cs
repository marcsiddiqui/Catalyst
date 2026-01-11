using Nop.Core.Domain.GenericDropDowns;
using Nop.Services.GenericDropDowns;
using Nop.Services.Localization;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.GenericDropDowns;
using Nop.Web.Framework.Factories;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Web.Areas.Admin.Factories;

public partial class GenericDropDownOptionModelFactory : IGenericDropDownOptionModelFactory
{
    #region Fields

    protected readonly IGenericDropDownOptionService _genericDropDownOptionService;
    protected readonly ILocalizationService _localizationService;
    protected readonly ILocalizedModelFactory _localizedModelFactory;
    protected readonly IStoreMappingSupportedModelFactory _storeMappingSupportedModelFactory;

    #endregion

    #region Ctor

    public GenericDropDownOptionModelFactory(IGenericDropDownOptionService genericDropDownOptionService,
        ILocalizationService localizationService,
        ILocalizedModelFactory localizedModelFactory,
        IStoreMappingSupportedModelFactory storeMappingSupportedModelFactory)
    {
        _genericDropDownOptionService = genericDropDownOptionService;
        _localizationService = localizationService;
        _localizedModelFactory = localizedModelFactory;
        _storeMappingSupportedModelFactory = storeMappingSupportedModelFactory;
    }

    #endregion

    #region Utilities

    

    #endregion

    #region Methods

    public virtual Task<GenericDropDownOptionSearchModel> PrepareGenericDropDownOptionSearchModelAsync(GenericDropDownOptionSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        //prepare page parameters
        searchModel.SetGridPageSize();

        return Task.FromResult(searchModel);
    }

    public virtual async Task<GenericDropDownOptionListModel> PrepareGenericDropDownOptionListModelAsync(GenericDropDownOptionSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        //get genericDropDownOptions
        var genericDropDownOptions = (await _genericDropDownOptionService.GetAllGenericDropDownOptionsAsync()).ToPagedList(searchModel);

        //prepare list model
        var model = await new GenericDropDownOptionListModel().PrepareToGridAsync(searchModel, genericDropDownOptions, () =>
        {
            //fill in model values from the entity
            return genericDropDownOptions.SelectAwait(async genericDropDownOption =>
            {
                var genericDropDownOptionModel = genericDropDownOption.ToModel<GenericDropDownOptionModel>();

                return genericDropDownOptionModel;
            });
        });

        return model;
    }

    public virtual async Task<GenericDropDownOptionModel> PrepareGenericDropDownOptionModelAsync(GenericDropDownOptionModel model, GenericDropDownOption genericDropDownOption, bool excludeProperties = false)
    {
        Func<GenericDropDownOptionLocalizedModel, int, Task> localizedModelConfiguration = null;

        if (genericDropDownOption != null)
        {
            //fill in model values from the entity
            if (model == null)
            {
                model = genericDropDownOption.ToModel<GenericDropDownOptionModel>();
            }

            //define localized model configuration action
            localizedModelConfiguration = async (locale, languageId) =>
            {
                locale.Text = await _localizationService.GetLocalizedAsync(genericDropDownOption, entity => entity.Text, languageId, false, false);


            };
        }

        //set default values for the new model
        if (genericDropDownOption == null)
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