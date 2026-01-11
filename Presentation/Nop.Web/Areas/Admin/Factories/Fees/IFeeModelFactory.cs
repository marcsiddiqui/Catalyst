using Nop.Core.Domain.Fees;
using Nop.Web.Areas.Admin.Models.Fees;

namespace Nop.Web.Areas.Admin.Factories;

public partial interface IFeeModelFactory
{
    Task<FeeSearchModel> PrepareFeeSearchModelAsync(FeeSearchModel searchModel);

    Task<FeeListModel> PrepareFeeListModelAsync(FeeSearchModel searchModel);

    Task<FeeModel> PrepareFeeModelAsync(FeeModel model, Fee fee, bool excludeProperties = false);
}