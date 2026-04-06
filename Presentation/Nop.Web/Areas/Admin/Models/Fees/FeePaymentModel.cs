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

    [NopResourceDisplayName("Admin.Configuration.FeePaymentses.Fields.CreatedBy")]
    public int CreatedBy { get; set; }

    [NopResourceDisplayName("Admin.Configuration.FeePaymentses.Fields.CreatedOnUtc")]
    public DateTime CreatedOnUtc { get; set; }

    [NopResourceDisplayName("Admin.Configuration.FeePaymentses.Fields.UpdatedBy")]
    public int UpdatedBy { get; set; }

    [NopResourceDisplayName("Admin.Configuration.FeePaymentses.Fields.UpdatedOnUtc")]
    public DateTime? UpdatedOnUtc { get; set; }



    #endregion
}

