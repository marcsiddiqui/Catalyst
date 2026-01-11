using Nop.Core;
using Nop.Core.Domain.Subjects;
using Nop.Data;

namespace Nop.Services.Subjects;

public partial class SubjectService : ISubjectService
{
    #region Fields

    protected readonly IRepository<Subject> _subjectRepository;

    #endregion

    #region Ctor

    public SubjectService(
        IRepository<Subject> subjectRepository
        )
    {
        _subjectRepository = subjectRepository;
    }

    #endregion

    #region Methods

    public virtual async Task<IPagedList<Subject>> GetAllSubjectsAsync(
        int id = 0, IEnumerable<int> ids = null,
        string name = null, IEnumerable<string> names = null,
        BooleanFilter isActive = BooleanFilter.True,
        BooleanFilter deleted = BooleanFilter.False,



        int pageIndex = 0, int pageSize = int.MaxValue)
    {
        var productReviews = await _subjectRepository.GetAllPagedAsync(async query =>
        {
            if (id > 0)
                query = query.Where(x => x.Id == id);

            if (ids != null && ids.Any())
                query = query.Where(x => ids.Contains(x.Id));

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(x => name == x.Name);

            if (names != null && names.Any())
                query = query.Where(x => names.Contains(x.Name));

            query = query.WhereBoolean(x => x.IsActive, isActive);

            query = query.WhereBoolean(x => x.Deleted, deleted);





            return query;

        }, pageIndex, pageSize);

        return productReviews;
    }

    public virtual async Task<Subject> GetSubjectByIdAsync(int id)
    {
        if (id <= 0)
            return null;

        return await _subjectRepository.GetByIdAsync(id);
    }

    public virtual async Task<IList<Subject>> GetSubjectsByIdsAsync(IEnumerable<int> ids)
    {
        if (ids == null || !ids.Any())
            return null;

        return await _subjectRepository.GetByIdsAsync(ids.ToList());
    }

    public virtual async Task InsertSubjectAsync(Subject subject)
    {
        if (subject == null)
            return;

        await _subjectRepository.InsertAsync(subject);
    }
    
    public virtual async Task InsertSubjectAsync(IEnumerable<Subject> subjects)
    {
        if (subjects == null || !subjects.Any())
            return;

        await _subjectRepository.InsertAsync(subjects.ToList());
    }

    public virtual async Task UpdateSubjectAsync(Subject subject)
    {
        if (subject == null)
            return;

        await _subjectRepository.UpdateAsync(subject);
    }

    public virtual async Task UpdateSubjectAsync(IEnumerable<Subject> subjects)
    {
        if (subjects == null || !subjects.Any())
            return;

        await _subjectRepository.UpdateAsync(subjects.ToList());
    }

    public virtual async Task DeleteSubjectAsync(Subject subject)
    {
        if (subject == null)
            return;

        await _subjectRepository.DeleteAsync(subject);
    }

    public virtual async Task DeleteSubjectAsync(IEnumerable<Subject> subjects)
    {
        if (subjects == null || !subjects.Any())
            return;

        await _subjectRepository.DeleteAsync(subjects.ToList());
    }

    #endregion
}