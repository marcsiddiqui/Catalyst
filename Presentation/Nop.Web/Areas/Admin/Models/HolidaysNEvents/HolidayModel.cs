using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace Nop.Web.Areas.Admin.Models.HolidaysNEvents;

public partial record HolidayModel : BaseNopEntityModel, ILocalizedModel<HolidayLocalizedModel>, IStoreMappingSupportedModel
{
    #region Ctor

    public HolidayModel()
    {
        Locales = [];

        SelectedStoreIds = [];
        AvailableStores = [];

        AvailableYears = [];
    }

    #endregion

    #region Properties

    [NopResourceDisplayName("Admin.Configuration.Holidays.Fields.AcademicYearId")]
    public int AcademicYearId { get; set; }
    public IList<SelectListItem> AvailableYears { get; set; }
    public string AcademicYear { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Holidays.Fields.Name")]
    public string Name { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Holidays.Fields.Description")]
    public string Description { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Holidays.Fields.DateFromUtc")]
    [UIHint("DateTimeNullable")]
    public DateTime? DateFromUtc { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Holidays.Fields.DateToUtc")]
    [UIHint("DateTimeNullable")]
    public DateTime? DateToUtc { get; set; }

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