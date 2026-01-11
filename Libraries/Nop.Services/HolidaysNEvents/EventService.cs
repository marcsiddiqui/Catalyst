using Nop.Core;
using Nop.Core.Domain.HolidaysNEvents;
using Nop.Data;

namespace Nop.Services.HolidaysNEvents;

public partial class EventService : IEventService
{
    #region Fields

    protected readonly IRepository<Event> _eventRepository;

    #endregion

    #region Ctor

    public EventService(
        IRepository<Event> eventRepository
        )
    {
        _eventRepository = eventRepository;
    }

    #endregion

    #region Methods

    public virtual async Task<IPagedList<Event>> GetAllEventsAsync(
        int id = 0, IEnumerable<int> ids = null,
        int academicYearId = 0, IEnumerable<int> academicYearIds = null,
        string name = null, IEnumerable<string> names = null,
        int storeId = 0, IEnumerable<int> storeIds = null,


        BooleanFilter deleted = BooleanFilter.False,
        BooleanFilter limitedToStores = BooleanFilter.Both,

        int pageIndex = 0, int pageSize = int.MaxValue)
    {
        var productReviews = await _eventRepository.GetAllPagedAsync(async query =>
        {
            if (id > 0)
                query = query.Where(x => x.Id == id);

            if (ids != null && ids.Any())
                query = query.Where(x => ids.Contains(x.Id));

            if (academicYearId > 0)
                query = query.Where(x => academicYearId == x.AcademicYearId);

            if (academicYearIds != null && academicYearIds.Any())
                query = query.Where(x => academicYearIds.Contains(x.AcademicYearId));

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(x => name == x.Name);

            if (names != null && names.Any())
                query = query.Where(x => names.Contains(x.Name));

            if (storeId > 0)
                query = query.Where(x => storeId == x.StoreId);

            if (storeIds != null && storeIds.Any())
                query = query.Where(x => storeIds.Contains(x.StoreId));



            query = query.WhereBoolean(x => x.Deleted, deleted);

            query = query.WhereBoolean(x => x.LimitedToStores, limitedToStores);



            return query;

        }, pageIndex, pageSize);

        return productReviews;
    }

    public virtual async Task<Event> GetEventByIdAsync(int id)
    {
        if (id <= 0)
            return null;

        return await _eventRepository.GetByIdAsync(id);
    }

    public virtual async Task<IList<Event>> GetEventsByIdsAsync(IEnumerable<int> ids)
    {
        if (ids == null || !ids.Any())
            return null;

        return await _eventRepository.GetByIdsAsync(ids.ToList());
    }

    public virtual async Task InsertEventAsync(Event @event)
    {
        if (@event == null)
            return;

        await _eventRepository.InsertAsync(@event);
    }
    
    public virtual async Task InsertEventAsync(IEnumerable<Event> events)
    {
        if (events == null || !events.Any())
            return;

        await _eventRepository.InsertAsync(events.ToList());
    }

    public virtual async Task UpdateEventAsync(Event @event)
    {
        if (@event == null)
            return;

        await _eventRepository.UpdateAsync(@event);
    }

    public virtual async Task UpdateEventAsync(IEnumerable<Event> events)
    {
        if (events == null || !events.Any())
            return;

        await _eventRepository.UpdateAsync(events.ToList());
    }

    public virtual async Task DeleteEventAsync(Event @event)
    {
        if (@event == null)
            return;

        await _eventRepository.DeleteAsync(@event);
    }

    public virtual async Task DeleteEventAsync(IEnumerable<Event> events)
    {
        if (events == null || !events.Any())
            return;

        await _eventRepository.DeleteAsync(events.ToList());
    }

    #endregion
}