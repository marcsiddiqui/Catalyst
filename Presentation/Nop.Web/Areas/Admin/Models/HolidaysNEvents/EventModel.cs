using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.HolidaysNEvents;

public partial record EventModel : BaseNopEntityModel, ILocalizedModel<EventLocalizedModel>, IStoreMappingSupportedModel
{
    #region Ctor

    public EventModel()
    {
        Locales = new List<EventLocalizedModel>();

        SelectedStoreIds = new List<int>();
        AvailableStores = new List<SelectListItem>();


    }

    #endregion

    #region Properties

    [NopResourceDisplayName("Admin.Configuration.Events.Fields.AcademicYearId")]
    public int AcademicYearId { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Events.Fields.Name")]
    public string Name { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Events.Fields.Description")]
    public string Description { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Events.Fields.StartDateUtc")]
    public DateTime StartDateUtc { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Events.Fields.EndDateUtc")]
    public DateTime EndDateUtc { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Events.Fields.StoreId")]
    public int StoreId { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Events.Fields.CreatedBy")]
    public int CreatedBy { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Events.Fields.CreatedOnUtc")]
    public DateTime CreatedOnUtc { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Events.Fields.UpdatedBy")]
    public int UpdatedBy { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Events.Fields.UpdatedOnUtc")]
    public DateTime? UpdatedOnUtc { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Events.Fields.Deleted")]
    public bool Deleted { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Events.Fields.LimitedToStores")]
    public bool LimitedToStores { get; set; }

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