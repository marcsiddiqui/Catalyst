using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Fees;

public partial record FeeModel : BaseNopEntityModel
{
    #region Ctor

    public FeeModel()
    {
        AvailableFeeTypes = new List<SelectListItem>();
        AvailableStudents = new List<SelectListItem>();
    }

    #endregion

    #region Properties

    [NopResourceDisplayName("Admin.Configuration.Fees.Fields.AcademicYearGradeSectionMappingId")]
    public int AcademicYearGradeSectionMappingId { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Fees.Fields.CustomerId")]
    public int CustomerId { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Fees.Fields.Amount")]
    public decimal Amount { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Fees.Fields.Discount")]
    public decimal Discount { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Fees.Fields.FeeTypeId")]
    public int FeeTypeId { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Fees.Fields.FeeDate")]
    public DateTime FeeDate { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Fees.Fields.Deleted")]
    public bool Deleted { get; set; }

    #endregion

    #region Extra Model Properties

    public IList<SelectListItem> AvailableFeeTypes { get; set; }

    public IList<SelectListItem> AvailableStudents { get; set; }

    public string StudentName { get; set; }

    public string FeeType { get; set; }

    public string FormattedFeeDate { get; set; }

    #endregion
}

