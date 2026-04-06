using Nop.Core;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Subjects;

namespace Nop.Services.Subjects;

public partial interface ISubjectService
{
    Task<IPagedList<Subject>> GetAllSubjectsAsync(
        int id = 0, IEnumerable<int> ids = null,
        string name = null, IEnumerable<string> names = null,
        BooleanFilter isActive = BooleanFilter.True,
        BooleanFilter deleted = BooleanFilter.False,



        int pageIndex = 0, int pageSize = int.MaxValue);

    Task<Subject> GetSubjectByIdAsync(int id);

    Task<IList<Subject>> GetSubjectsByIdsAsync(IEnumerable<int> ids);

    Task InsertSubjectAsync(Subject subject);
    
    Task InsertSubjectAsync(IEnumerable<Subject> subjects);

    Task UpdateSubjectAsync(Subject subject);

    Task UpdateSubjectAsync(IEnumerable<Subject> subjects);

    Task DeleteSubjectAsync(Subject subject);

    Task DeleteSubjectAsync(IEnumerable<Subject> subjects);
}