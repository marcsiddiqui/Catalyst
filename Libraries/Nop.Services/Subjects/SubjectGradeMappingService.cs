using Nop.Core;
using Nop.Core.Domain.Subjects;
using Nop.Data;

namespace Nop.Services.Subjects;

public partial class SubjectGradeMappingService : ISubjectGradeMappingService
{
    #region Fields

    protected readonly IRepository<SubjectGradeMapping> _subjectGradeMappingRepository;

    #endregion

    #region Ctor

    public SubjectGradeMappingService(
        IRepository<SubjectGradeMapping> subjectGradeMappingRepository
        )
    {
        _subjectGradeMappingRepository = subjectGradeMappingRepository;
    }

    #endregion

    #region Methods

    public virtual async Task<IPagedList<SubjectGradeMapping>> GetAllSubjectGradeMappingsAsync(
        int id = 0, IEnumerable<int> ids = null,
        int gradeId = 0, IEnumerable<int> gradeIds = null,
        int subjectId = 0, IEnumerable<int> subjectIds = null,
        int sectionId = 0, IEnumerable<int> sectionIds = null,

        int pageIndex = 0, int pageSize = int.MaxValue)
    {
        var productReviews = await _subjectGradeMappingRepository.GetAllPagedAsync(async query =>
        {
            if (id > 0)
                query = query.Where(x => x.Id == id);

            if (ids != null && ids.Any())
                query = query.Where(x => ids.Contains(x.Id));

            if (gradeId > 0)
                query = query.Where(x => gradeId == x.GradeId);

            if (gradeIds != null && gradeIds.Any())
                query = query.Where(x => gradeIds.Contains(x.GradeId));

            if (subjectId > 0)
                query = query.Where(x => subjectId == x.SubjectId);

            if (subjectIds != null && subjectIds.Any())
                query = query.Where(x => subjectIds.Contains(x.SubjectId));

            if (sectionId > 0)
                query = query.Where(x => sectionId == x.SectionId);

            if (sectionIds != null && sectionIds.Any())
                query = query.Where(x => sectionIds.Contains(x.SectionId.Value));



            return query;

        }, pageIndex, pageSize);

        return productReviews;
    }

    public virtual async Task<SubjectGradeMapping> GetSubjectGradeMappingByIdAsync(int id)
    {
        if (id <= 0)
            return null;

        return await _subjectGradeMappingRepository.GetByIdAsync(id);
    }

    public virtual async Task<IList<SubjectGradeMapping>> GetSubjectGradeMappingsByIdsAsync(IEnumerable<int> ids)
    {
        if (ids == null || !ids.Any())
            return null;

        return await _subjectGradeMappingRepository.GetByIdsAsync(ids.ToList());
    }

    public virtual async Task InsertSubjectGradeMappingAsync(SubjectGradeMapping subjectGradeMapping)
    {
        if (subjectGradeMapping == null)
            return;

        await _subjectGradeMappingRepository.InsertAsync(subjectGradeMapping);
    }
    
    public virtual async Task InsertSubjectGradeMappingAsync(IEnumerable<SubjectGradeMapping> subjectGradeMappings)
    {
        if (subjectGradeMappings == null || !subjectGradeMappings.Any())
            return;

        await _subjectGradeMappingRepository.InsertAsync(subjectGradeMappings.ToList());
    }

    public virtual async Task UpdateSubjectGradeMappingAsync(SubjectGradeMapping subjectGradeMapping)
    {
        if (subjectGradeMapping == null)
            return;

        await _subjectGradeMappingRepository.UpdateAsync(subjectGradeMapping);
    }

    public virtual async Task UpdateSubjectGradeMappingAsync(IEnumerable<SubjectGradeMapping> subjectGradeMappings)
    {
        if (subjectGradeMappings == null || !subjectGradeMappings.Any())
            return;

        await _subjectGradeMappingRepository.UpdateAsync(subjectGradeMappings.ToList());
    }

    public virtual async Task DeleteSubjectGradeMappingAsync(SubjectGradeMapping subjectGradeMapping)
    {
        if (subjectGradeMapping == null)
            return;

        await _subjectGradeMappingRepository.DeleteAsync(subjectGradeMapping);
    }

    public virtual async Task DeleteSubjectGradeMappingAsync(IEnumerable<SubjectGradeMapping> subjectGradeMappings)
    {
        if (subjectGradeMappings == null || !subjectGradeMappings.Any())
            return;

        await _subjectGradeMappingRepository.DeleteAsync(subjectGradeMappings.ToList());
    }

    #endregion
}