using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.HolidaysNEvents;

public partial record HolidayModel : BaseNopEntityModel, ILocalizedModel<HolidayLocalizedModel>, IStoreMappingSupportedModel
{
    #region Ctor

    public HolidayModel()
    {
        Locales = new List<HolidayLocalizedModel>();

        SelectedStoreIds = new List<int>();
        AvailableStores = new List<SelectListItem>();


    }

    #endregion

    #region Properties

    [NopResourceDisplayName("Admin.Configuration.Holidays.Fields.AcademicYearId")]
    public int AcademicYearId { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Holidays.Fields.Name")]
    public string Name { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Holidays.Fields.Description")]
    public string Description { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Holidays.Fields.DateFromUtc")]
    public DateTime DateFromUtc { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Holidays.Fields.DateToUtc")]
    public DateTime DateToUtc { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Holidays.Fields.StoreId")]
    public int StoreId { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Holidays.Fields.CreatedBy")]
    public int CreatedBy { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Holidays.Fields.CreatedOnUtc")]
    public DateTime CreatedOnUtc { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Holidays.Fields.UpdatedBy")]
    public int UpdatedBy { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Holidays.Fields.UpdatedOnUtc")]
    public DateTime? UpdatedOnUtc { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Holidays.Fields.Deleted")]
    public bool Deleted { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Holidays.Fields.LimitedToStores")]
    public bool LimitedToStores { get; set; }

    public IList<HolidayLocalizedModel> Locales { get; set; }

    //store mapping
    [NopResourceDisplayName("Admin.Configuration.Holidays.Fields.LimitedToStores")]
    public IList<int> SelectedStoreIds { get; set; }
    public IList<SelectListItem> AvailableStores { get; set; }



    #endregion
}

public partial record HolidayLocalizedModel : ILocalizedLocaleModel
{
    public int LanguageId { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Holidays.Fields.Name")]
    public string Name { get; set; }


}