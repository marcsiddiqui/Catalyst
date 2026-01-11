using Nop.Core;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.HolidaysNEvents;

namespace Nop.Services.HolidaysNEvents;

public partial interface IEventService
{
    Task<IPagedList<Event>> GetAllEventsAsync(
        int id = 0, IEnumerable<int> ids = null,
        int academicYearId = 0, IEnumerable<int> academicYearIds = null,
        string name = null, IEnumerable<string> names = null,
        int storeId = 0, IEnumerable<int> storeIds = null,


        BooleanFilter deleted = BooleanFilter.False,
        BooleanFilter limitedToStores = BooleanFilter.Both,

        int pageIndex = 0, int pageSize = int.MaxValue);

    Task<Event> GetEventByIdAsync(int id);

    Task<IList<Event>> GetEventsByIdsAsync(IEnumerable<int> ids);

    Task InsertEventAsync(Event @event);
    
    Task InsertEventAsync(IEnumerable<Event> events);

    Task UpdateEventAsync(Event @event);

    Task UpdateEventAsync(IEnumerable<Event> events);

    Task DeleteEventAsync(Event @event);

    Task DeleteEventAsync(IEnumerable<Event> events);
}