using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace Nop.Web.Areas.Admin.Models.HolidaysNEvents;

public partial record EventModel : BaseNopEntityModel, ILocalizedModel<EventLocalizedModel>, IStoreMappingSupportedModel
{
    #region Ctor

    public EventModel()
    {
        Locales = [];

        SelectedStoreIds = [];
        AvailableStores = [];

        AvailableYears = [];
    }

    #endregion

    #region Properties

    [NopResourceDisplayName("Admin.Configuration.Events.Fields.AcademicYearId")]
    public int AcademicYearId { get; set; }
    public IList<SelectListItem> AvailableYears { get; set; }
    public string AcademicYear { get; set; }


    [NopResourceDisplayName("Admin.Configuration.Events.Fields.Name")]
    public string Name { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Events.Fields.Description")]
    public string Description { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Events.Fields.StartDateUtc")]
    [UIHint("DateTimeNullable")]
    public DateTime? StartDateUtc { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Events.Fields.EndDateUtc")]
    [UIHint("DateTimeNullable")]
    public DateTime? EndDateUtc { get; set; }

    public IList<EventLocalizedModel> Locales { get; set; }

    //store mapping
    [NopResourceDisplayName("Admin.Configuration.Events.Fields.LimitedToStores")]
    public IList<int> SelectedStoreIds { get; set; }
    public IList<SelectListItem> AvailableStores { get; set; }


    #endregion
}

public partial record EventLocalizedModel : ILocalizedLocaleModel
{
    public int LanguageId { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Events.Fields.Name")]
    public string Name { get; set; }


}