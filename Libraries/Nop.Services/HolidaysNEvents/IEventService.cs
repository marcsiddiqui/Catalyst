using Nop.Core;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.HolidaysNEvents;

namespace Nop.Services.HolidaysNEvents;

public partial interface IEventService
{
    #region Event

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

    #endregion

    #region AcademicYearGradeSectionEventMapping

    Task<IPagedList<AcademicYearGradeSectionEventMapping>> GetAllAcademicYearGradeSectionEventMappingsAsync(
        int id = 0, IEnumerable<int> ids = null,
        int eventId = 0, IEnumerable<int> eventIds = null,
        int academicYearGradeSectionMappingId = 0, IEnumerable<int> academicYearGradeSectionMappingIds = null,

        int pageIndex = 0, int pageSize = int.MaxValue);

    Task<AcademicYearGradeSectionEventMapping> GetAcademicYearGradeSectionEventMappingByIdAsync(int id);

    Task<IList<AcademicYearGradeSectionEventMapping>> GetAcademicYearGradeSectionEventMappingsByIdsAsync(IEnumerable<int> ids);

    Task InsertAcademicYearGradeSectionEventMappingAsync(AcademicYearGradeSectionEventMapping academicYearGradeSectionEventMapping);

    Task InsertAcademicYearGradeSectionEventMappingAsync(IEnumerable<AcademicYearGradeSectionEventMapping> academicYearGradeSectionEventMappings);

    Task UpdateAcademicYearGradeSectionEventMappingAsync(AcademicYearGradeSectionEventMapping academicYearGradeSectionEventMapping);

    Task UpdateAcademicYearGradeSectionEventMappingAsync(IEnumerable<AcademicYearGradeSectionEventMapping> academicYearGradeSectionEventMappings);

    Task DeleteAcademicYearGradeSectionEventMappingAsync(AcademicYearGradeSectionEventMapping academicYearGradeSectionEventMapping);

    Task DeleteAcademicYearGradeSectionEventMappingAsync(IEnumerable<AcademicYearGradeSectionEventMapping> academicYearGradeSectionEventMappings);

    #endregion

    #region StudentEventMapping

    Task<IPagedList<StudentEventMapping>> GetAllStudentEventMappingsAsync(
        int id = 0, IEnumerable<int> ids = null,
        int eventId = 0, IEnumerable<int> eventIds = null,
        int customerId = 0, IEnumerable<int> customerIds = null,



        int pageIndex = 0, int pageSize = int.MaxValue);

    Task<StudentEventMapping> GetStudentEventMappingByIdAsync(int id);

    Task<IList<StudentEventMapping>> GetStudentEventMappingsByIdsAsync(IEnumerable<int> ids);

    Task InsertStudentEventMappingAsync(StudentEventMapping studentEventMapping);

    Task InsertStudentEventMappingAsync(IEnumerable<StudentEventMapping> studentEventMappings);

    Task UpdateStudentEventMappingAsync(StudentEventMapping studentEventMapping);

    Task UpdateStudentEventMappingAsync(IEnumerable<StudentEventMapping> studentEventMappings);

    Task DeleteStudentEventMappingAsync(StudentEventMapping studentEventMapping);

    Task DeleteStudentEventMappingAsync(IEnumerable<StudentEventMapping> studentEventMappings);

    #endregion
}