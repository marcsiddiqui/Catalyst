using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.GradeManagement;

public partial record GradeModel : BaseNopEntityModel, ILocalizedModel<GradeLocalizedModel>, IStoreMappingSupportedModel
{
    #region Ctor

    public GradeModel()
    {
        Locales = new List<GradeLocalizedModel>();

        SelectedStoreIds = new List<int>();
        AvailableStores = new List<SelectListItem>();


    }

    #endregion

    #region Properties

    [NopResourceDisplayName("Admin.Configuration.Grades.Fields.Name")]
    public string Name { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Grades.Fields.IsActive")]
    public bool IsActive { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Grades.Fields.Deleted")]
    public bool Deleted { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Grades.Fields.BaseFeeAmount")]
    public decimal BaseFeeAmount { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Grades.Fields.StoreId")]
    public int StoreId { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Grades.Fields.CreatedBy")]
    public int CreatedBy { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Grades.Fields.CreatedOnUtc")]
    public DateTime CreatedOnUtc { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Grades.Fields.UpdatedBy")]
    public int UpdatedBy { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Grades.Fields.UpdatedOnUtc")]
    public DateTime? UpdatedOnUtc { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Grades.Fields.LimitedToStores")]
    public bool LimitedToStores { get; set; }

    public IList<GradeLocalizedModel> Locales { get; set; }

    //store mapping
    [NopResourceDisplayName("Admin.Configuration.Grades.Fields.LimitedToStores")]
    public IList<int> SelectedStoreIds { get; set; }
    public IList<SelectListItem> AvailableStores { get; set; }



    #endregion
}

public partial record GradeLocalizedModel : ILocalizedLocaleModel
{
    public int LanguageId { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Grades.Fields.Name")]
    public string Name { get; set; }


}