using DocumentFormat.OpenXml.Vml.Spreadsheet;
using Nop.Core;
using Nop.Core.Domain.HolidaysNEvents;
using Nop.Data;
using Nop.Services.Stores;

namespace Nop.Services.HolidaysNEvents;

public partial class EventService : IEventService
{
    #region Fields

    protected readonly IRepository<Event> _eventRepository;
    protected readonly IRepository<AcademicYearGradeSectionEventMapping> _academicYearGradeSectionEventMappingRepository;
    protected readonly IRepository<StudentEventMapping> _studentEventMappingRepository;
    protected readonly IStoreMappingService _storeMappingService;

    #endregion

    #region Ctor

    public EventService(
        IRepository<Event> eventRepository,
        IRepository<AcademicYearGradeSectionEventMapping> academicYearGradeSectionEventMappingRepository,
        IRepository<StudentEventMapping> studentEventMappingRepository,
        IStoreMappingService storeMappingService)
    {
        _eventRepository = eventRepository;
        _academicYearGradeSectionEventMappingRepository = academicYearGradeSectionEventMappingRepository;
        _studentEventMappingRepository = studentEventMappingRepository;
        _storeMappingService = storeMappingService;
    }

    #endregion

    #region Methods

    #region Event

    public virtual async Task<IPagedList<Event>> GetAllEventsAsync(
        int academicYearId = default,
        string name = default,
        int storeId = default,
        int pageIndex = default,
        int pageSize = int.MaxValue)
    {
        var productReviews = await _eventRepository.GetAllPagedAsync(async query =>
        {
            if (academicYearId > 0)
                query = query.Where(x => academicYearId == x.AcademicYearId);

            if (!CommonHelper.IsDefault(name))
                query = query.Where(x => name.Contains(x.Name));

            if (storeId > 0)
            {
                //apply store mapping constraints
                query = await _storeMappingService.ApplyStoreMapping(query, storeId);
            }

            return query;

        }, pageIndex, pageSize, includeDeleted: false);

        return productReviews;
    }

    public virtual async Task<Event> GetEventByIdAsync(int id)
    {
        return await _eventRepository.GetByIdAsync(id);
    }

    public virtual async Task<IList<Event>> GetEventsByIdsAsync(IEnumerable<int> ids)
    {
        return await _eventRepository.GetByIdsAsync(ids.ToList());
    }

    public virtual async Task InsertEventAsync(Event @event)
    {
        await _eventRepository.InsertAsync(@event);
    }

    public virtual async Task InsertEventAsync(IEnumerable<Event> events)
    {
        await _eventRepository.InsertAsync(events.ToList());
    }

    public virtual async Task UpdateEventAsync(Event @event)
    {
        await _eventRepository.UpdateAsync(@event);
    }

    public virtual async Task UpdateEventAsync(IEnumerable<Event> events)
    {
        await _eventRepository.UpdateAsync(events.ToList());
    }

    public virtual async Task DeleteEventAsync(Event @event)
    {
        await _eventRepository.DeleteAsync(@event);
    }

    public virtual async Task DeleteEventAsync(IEnumerable<Event> events)
    {
        await _eventRepository.DeleteAsync(events.ToList());
    }

    #endregion

    #region AcademicYearGradeSectionEventMapping

    public virtual async Task<IPagedList<AcademicYearGradeSectionEventMapping>> GetAllAcademicYearGradeSectionEventMappingsAsync(
        int eventId = 0,
        IEnumerable<int> eventIds = null,
        int academicYearGradeSectionMappingId = 0,
        IEnumerable<int> academicYearGradeSectionMappingIds = null,
        int pageIndex = 0,
        int pageSize = int.MaxValue)
    {
        var productReviews = await _academicYearGradeSectionEventMappingRepository.GetAllPagedAsync(async query =>
        {
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
        return await _academicYearGradeSectionEventMappingRepository.GetByIdAsync(id);
    }

    public virtual async Task<IList<AcademicYearGradeSectionEventMapping>> GetAcademicYearGradeSectionEventMappingsByIdsAsync(IEnumerable<int> ids)
    {
        return await _academicYearGradeSectionEventMappingRepository.GetByIdsAsync(ids.ToList());
    }

    public virtual async Task InsertAcademicYearGradeSectionEventMappingAsync(AcademicYearGradeSectionEventMapping academicYearGradeSectionEventMapping)
    {
        await _academicYearGradeSectionEventMappingRepository.InsertAsync(academicYearGradeSectionEventMapping);
    }

    public virtual async Task InsertAcademicYearGradeSectionEventMappingAsync(IEnumerable<AcademicYearGradeSectionEventMapping> academicYearGradeSectionEventMappings)
    {
        await _academicYearGradeSectionEventMappingRepository.InsertAsync(academicYearGradeSectionEventMappings.ToList());
    }

    public virtual async Task UpdateAcademicYearGradeSectionEventMappingAsync(AcademicYearGradeSectionEventMapping academicYearGradeSectionEventMapping)
    {
        await _academicYearGradeSectionEventMappingRepository.UpdateAsync(academicYearGradeSectionEventMapping);
    }

    public virtual async Task UpdateAcademicYearGradeSectionEventMappingAsync(IEnumerable<AcademicYearGradeSectionEventMapping> academicYearGradeSectionEventMappings)
    {
        await _academicYearGradeSectionEventMappingRepository.UpdateAsync(academicYearGradeSectionEventMappings.ToList());
    }

    public virtual async Task DeleteAcademicYearGradeSectionEventMappingAsync(AcademicYearGradeSectionEventMapping academicYearGradeSectionEventMapping)
    {
        await _academicYearGradeSectionEventMappingRepository.DeleteAsync(academicYearGradeSectionEventMapping);
    }

    public virtual async Task DeleteAcademicYearGradeSectionEventMappingAsync(IEnumerable<AcademicYearGradeSectionEventMapping> academicYearGradeSectionEventMappings)
    {
        if (academicYearGradeSectionEventMappings == null || !academicYearGradeSectionEventMappings.Any())
            return;

        await _academicYearGradeSectionEventMappingRepository.DeleteAsync(academicYearGradeSectionEventMappings.ToList());
    }

    #endregion

    #region StudentEventMapping

    public virtual async Task<IPagedList<StudentEventMapping>> GetAllStudentEventMappingsAsync(
        int eventId = 0,
        IEnumerable<int> eventIds = null,
        int customerId = 0,
        IEnumerable<int> customerIds = null,
        int pageIndex = 0,
        int pageSize = int.MaxValue)
    {
        var productReviews = await _studentEventMappingRepository.GetAllPagedAsync(async query =>
        {
            if (eventId > 0)
                query = query.Where(x => eventId == x.EventId);

            if (eventIds != null && eventIds.Any())
                query = query.Where(x => eventIds.Contains(x.EventId));

            if (customerId > 0)
                query = query.Where(x => customerId == x.CustomerId);

            if (customerIds != null && customerIds.Any())
                query = query.Where(x => customerIds.Contains(x.CustomerId));

            return query;

        }, pageIndex, pageSize);

        return productReviews;
    }

    public virtual async Task<StudentEventMapping> GetStudentEventMappingByIdAsync(int id)
    {
        return await _studentEventMappingRepository.GetByIdAsync(id);
    }

    public virtual async Task<IList<StudentEventMapping>> GetStudentEventMappingsByIdsAsync(IEnumerable<int> ids)
    {
        return await _studentEventMappingRepository.GetByIdsAsync(ids.ToList());
    }

    public virtual async Task InsertStudentEventMappingAsync(StudentEventMapping studentEventMapping)
    {
        await _studentEventMappingRepository.InsertAsync(studentEventMapping);
    }

    public virtual async Task InsertStudentEventMappingAsync(IEnumerable<StudentEventMapping> studentEventMappings)
    {
        await _studentEventMappingRepository.InsertAsync(studentEventMappings.ToList());
    }

    public virtual async Task UpdateStudentEventMappingAsync(StudentEventMapping studentEventMapping)
    {
        await _studentEventMappingRepository.UpdateAsync(studentEventMapping);
    }

    public virtual async Task UpdateStudentEventMappingAsync(IEnumerable<StudentEventMapping> studentEventMappings)
    {
        await _studentEventMappingRepository.UpdateAsync(studentEventMappings.ToList());
    }

    public virtual async Task DeleteStudentEventMappingAsync(StudentEventMapping studentEventMapping)
    {
        await _studentEventMappingRepository.DeleteAsync(studentEventMapping);
    }

    public virtual async Task DeleteStudentEventMappingAsync(IEnumerable<StudentEventMapping> studentEventMappings)
    {
        await _studentEventMappingRepository.DeleteAsync(studentEventMappings.ToList());
    }

    #endregion

    #endregion
}