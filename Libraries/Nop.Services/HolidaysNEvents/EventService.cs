using Nop.Core;
using Nop.Core.Domain.HolidaysNEvents;
using Nop.Data;

namespace Nop.Services.HolidaysNEvents;

public partial class EventService : IEventService
{
    #region Fields

    protected readonly IRepository<Event> _eventRepository;
    protected readonly IRepository<AcademicYearGradeSectionEventMapping> _academicYearGradeSectionEventMappingRepository;

    #endregion

    #region Ctor

    public EventService(
        IRepository<Event> eventRepository,
        IRepository<AcademicYearGradeSectionEventMapping> academicYearGradeSectionEventMappingRepository
        )
    {
        _eventRepository = eventRepository;
        _academicYearGradeSectionEventMappingRepository = academicYearGradeSectionEventMappingRepository;
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

    #region AcademicYearGradeSectionEventMapping

    public virtual async Task<IPagedList<AcademicYearGradeSectionEventMapping>> GetAllAcademicYearGradeSectionEventMappingsAsync(
        int id = 0, IEnumerable<int> ids = null,
        int eventId = 0, IEnumerable<int> eventIds = null,
        int academicYearGradeSectionMappingId = 0, IEnumerable<int> academicYearGradeSectionMappingIds = null,

        int pageIndex = 0, int pageSize = int.MaxValue)
    {
        var productReviews = await _academicYearGradeSectionEventMappingRepository.GetAllPagedAsync(async query =>
        {
            if (id > 0)
                query = query.Where(x => x.Id == id);

            if (ids != null && ids.Any())
                query = query.Where(x => ids.Contains(x.Id));

            if (eventId > 0)
                query = query.Where(x => eventId == x.EventId);

            if (eventIds != null && eventIds.Any())
                query = query.Where(x => eventIds.Contains(x.EventId));

            if (academicYearGradeSectionMappingId > 0)
                query = query.Where(x => academicYearGradeSectionMappingId == x.AcademicYearGradeSectionMappingId);

            if (academicYearGradeSectionMappingIds != null && academicYearGradeSectionMappingIds.Any())
                query = query.Where(x => academicYearGradeSectionMappingIds.Contains(x.AcademicYearGradeSectionMappingId));



            return query;

        }, pageIndex, pageSize);

        return productReviews;
    }

    public virtual async Task<AcademicYearGradeSectionEventMapping> GetAcademicYearGradeSectionEventMappingByIdAsync(int id)
    {
        if (id <= 0)
            return null;

        return await _academicYearGradeSectionEventMappingRepository.GetByIdAsync(id);
    }

    public virtual async Task<IList<AcademicYearGradeSectionEventMapping>> GetAcademicYearGradeSectionEventMappingsByIdsAsync(IEnumerable<int> ids)
    {
        if (ids == null || !ids.Any())
            return null;

        return await _academicYearGradeSectionEventMappingRepository.GetByIdsAsync(ids.ToList());
    }

    public virtual async Task InsertAcademicYearGradeSectionEventMappingAsync(AcademicYearGradeSectionEventMapping academicYearGradeSectionEventMapping)
    {
        if (academicYearGradeSectionEventMapping == null)
            return;

        await _academicYearGradeSectionEventMappingRepository.InsertAsync(academicYearGradeSectionEventMapping);
    }

    public virtual async Task InsertAcademicYearGradeSectionEventMappingAsync(IEnumerable<AcademicYearGradeSectionEventMapping> academicYearGradeSectionEventMappings)
    {
        if (academicYearGradeSectionEventMappings == null || !academicYearGradeSectionEventMappings.Any())
            return;

        await _academicYearGradeSectionEventMappingRepository.InsertAsync(academicYearGradeSectionEventMappings.ToList());
    }

    public virtual async Task UpdateAcademicYearGradeSectionEventMappingAsync(AcademicYearGradeSectionEventMapping academicYearGradeSectionEventMapping)
    {
        if (academicYearGradeSectionEventMapping == null)
            return;

        await _academicYearGradeSectionEventMappingRepository.UpdateAsync(academicYearGradeSectionEventMapping);
    }

    public virtual async Task UpdateAcademicYearGradeSectionEventMappingAsync(IEnumerable<AcademicYearGradeSectionEventMapping> academicYearGradeSectionEventMappings)
    {
        if (academicYearGradeSectionEventMappings == null || !academicYearGradeSectionEventMappings.Any())
            return;

        await _academicYearGradeSectionEventMappingRepository.UpdateAsync(academicYearGradeSectionEventMappings.ToList());
    }

    public virtual async Task DeleteAcademicYearGradeSectionEventMappingAsync(AcademicYearGradeSectionEventMapping academicYearGradeSectionEventMapping)
    {
        if (academicYearGradeSectionEventMapping == null)
            return;

        await _academicYearGradeSectionEventMappingRepository.DeleteAsync(academicYearGradeSectionEventMapping);
    }

    public virtual async Task DeleteAcademicYearGradeSectionEventMappingAsync(IEnumerable<AcademicYearGradeSectionEventMapping> academicYearGradeSectionEventMappings)
    {
        if (academicYearGradeSectionEventMappings == null || !academicYearGradeSectionEventMappings.Any())
            return;

        await _academicYearGradeSectionEventMappingRepository.DeleteAsync(academicYearGradeSectionEventMappings.ToList());
    }

    #endregion
}