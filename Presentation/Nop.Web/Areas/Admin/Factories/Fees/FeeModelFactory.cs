using Nop.Core.Domain.Fees;
using Nop.Services.Fees;
using Nop.Services.Localization;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.Fees;
using Nop.Web.Framework.Factories;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Web.Areas.Admin.Factories;

public partial class FeeModelFactory : IFeeModelFactory
{
    #region Fields

    protected readonly IFeeService _feeService;
    protected readonly ILocalizationService _localizationService;
    protected readonly ILocalizedModelFactory _localizedModelFactory;
    protected readonly IStoreMappingSupportedModelFactory _storeMappingSupportedModelFactory;

    #endregion

    #region Ctor

    public FeeModelFactory(IFeeService feeService,
        ILocalizationService localizationService,
        ILocalizedModelFactory localizedModelFactory,
        IStoreMappingSupportedModelFactory storeMappingSupportedModelFactory)
    {
        _feeService = feeService;
        _localizationService = localizationService;
        _localizedModelFactory = localizedModelFactory;
        _storeMappingSupportedModelFactory = storeMappingSupportedModelFactory;
    }

    #endregion

    #region Utilities

    

    #endregion

    #region Methods

    public virtual Task<FeeSearchModel> PrepareFeeSearchModelAsync(FeeSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        //prepare page parameters
        searchModel.SetGridPageSize();

        return Task.FromResult(searchModel);
    }

    public virtual async Task<FeeListModel> PrepareFeeListModelAsync(FeeSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        //get fees
        var fees = (await _feeService.GetAllFeesAsync()).ToPagedList(searchModel);

        //prepare list model
        var model = await new FeeListModel().PrepareToGridAsync(searchModel, fees, () =>
        {
            //fill in model values from the entity
            return fees.SelectAwait(async fee =>
            {
                var feeModel = fee.ToModel<FeeModel>();

                return feeModel;
            });
        });

        return model;
    }

    public virtual async Task<FeeModel> PrepareFeeModelAsync(FeeModel model, Fee fee, bool excludeProperties = false)
    {
//{{LocalizedModelObject}}

        if (fee != null)
        {
            //fill in model values from the entity
            if (model == null)
            {
                model = fee.ToModel<FeeModel>();
            }


        }

        //set default values for the new model
        if (fee == null)
        {
            
        }

//{{Locales}}

//{{PrepareStoreCode}}

        return model;
    }

    #endregion
}