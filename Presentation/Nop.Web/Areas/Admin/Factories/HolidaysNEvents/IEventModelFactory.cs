using Nop.Core.Domain.HolidaysNEvents;
using Nop.Web.Areas.Admin.Models.HolidaysNEvents;

namespace Nop.Web.Areas.Admin.Factories;

public partial interface IEventModelFactory
{
    Task<EventSearchModel> PrepareEventSearchModelAsync(EventSearchModel searchModel);

    Task<EventListModel> PrepareEventListModelAsync(EventSearchModel searchModel);

    Task<EventModel> PrepareEventModelAsync(EventModel model, Event @event, bool excludeProperties = false);
}