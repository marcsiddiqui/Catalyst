using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.HolidaysNEvents;

public partial record HolidaySearchModel : BaseSearchModel
{
    public HolidaySearchModel()
    {
        AvailableStores = [];
        AvailableYears = [];
    }

    [NopResourceDisplayName("Admin.Catalog.Holidays.Fields.SearchStore")]
    public int SearchStoreId { get; set; }
    public IList<SelectListItem> AvailableStores { get; set; }


    [NopResourceDisplayName("Admin.Catalog.Holidays.Fields.SearchStore")]
    public int SearchYearId { get; set; }
    public IList<SelectListItem> AvailableYears { get; set; }
}