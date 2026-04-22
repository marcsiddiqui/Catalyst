using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Fees;

public partial record FeePaymentModel : BaseNopEntityModel
{
    #region Ctor

    public FeePaymentModel()
    {

    }

    #endregion

    #region Properties

    [NopResourceDisplayName("Admin.Configuration.FeePaymentses.Fields.FeeId")]
    public int FeeId { get; set; }

    [NopResourceDisplayName("Admin.Configuration.FeePaymentses.Fields.PaidAmount")]
    public decimal PaidAmount { get; set; }

    [NopResourceDisplayName("Admin.Configuration.FeePaymentses.Fields.StatusId")]
    public int StatusId { get; set; }

    #endregion
}

