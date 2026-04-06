using Nop.Core;
using Nop.Core.Domain.Students;
using Nop.Data;

namespace Nop.Services.Students;

public partial class AcademicYearGradeSectionStudentMappingService : IAcademicYearGradeSectionStudentMappingService
{
    #region Fields

    protected readonly IRepository<AcademicYearGradeSectionStudentMapping> _academicYearGradeSectionStudentMappingRepository;

    #endregion

    #region Ctor

    public AcademicYearGradeSectionStudentMappingService(
        IRepository<AcademicYearGradeSectionStudentMapping> academicYearGradeSectionStudentMappingRepository
        )
    {
        _academicYearGradeSectionStudentMappingRepository = academicYearGradeSectionStudentMappingRepository;
    }

    #endregion

    #region Methods

    public virtual async Task<IPagedList<AcademicYearGradeSectionStudentMapping>> GetAllAcademicYearGradeSectionStudentMappingsAsync(
        int id = 0, IEnumerable<int> ids = null,
        int academicYearGradeSectionMappingId = 0, IEnumerable<int> academicYearGradeSectionMappingIds = null,
        int customerId = 0, IEnumerable<int> customerIds = null,
        int statusId = 0, IEnumerable<int> statusIds = null,
        string grade = null, IEnumerable<string> grades = null,

        int pageIndex = 0, int pageSize = int.MaxValue)
    {
        var productReviews = await _academicYearGradeSectionStudentMappingRepository.GetAllPagedAsync(async query =>
        {
            if (id > 0)
                query = query.Where(x => x.Id == id);

            if (ids != null && ids.Any())
                query = query.Where(x => ids.Contains(x.Id));

            if (academicYearGradeSectionMappingId > 0)
                query = query.Where(x => academicYearGradeSectionMappingId == x.AcademicYearGradeSectionMappingId);

            if (academicYearGradeSectionMappingIds != null && academicYearGradeSectionMappingIds.Any())
                query = query.Where(x => academicYearGradeSectionMappingIds.Contains(x.AcademicYearGradeSectionMappingId));

            if (customerId > 0)
                query = query.Where(x => customerId == x.CustomerId);

            if (customerIds != null && customerIds.Any())
                query = query.Where(x => customerIds.Contains(x.CustomerId));

            if (statusId > 0)
                query = query.Where(x => statusId == x.StatusId);

            if (statusIds != null && statusIds.Any())
                query = query.Where(x => statusIds.Contains(x.StatusId));

            if (!string.IsNullOrWhiteSpace(grade))
                query = query.Where(x => grade == x.Grade);

            if (grades != null && grades.Any())
                query = query.Where(x => grades.Contains(x.Grade));



            return query;

        }, pageIndex, pageSize);

        return productReviews;
    }

    public virtual async Task<AcademicYearGradeSectionStudentMapping> GetAcademicYearGradeSectionStudentMappingByIdAsync(int id)
    {
        if (id <= 0)
            return null;

        return await _academicYearGradeSectionStudentMappingRepository.GetByIdAsync(id);
    }

    public virtual async Task<IList<AcademicYearGradeSectionStudentMapping>> GetAcademicYearGradeSectionStudentMappingsByIdsAsync(IEnumerable<int> ids)
    {
        if (ids == null || !ids.Any())
            return null;

        return await _academicYearGradeSectionStudentMappingRepository.GetByIdsAsync(ids.ToList());
    }

    public virtual async Task InsertAcademicYearGradeSectionStudentMappingAsync(AcademicYearGradeSectionStudentMapping academicYearGradeSectionStudentMapping)
    {
        if (academicYearGradeSectionStudentMapping == null)
            return;

        await _academicYearGradeSectionStudentMappingRepository.InsertAsync(academicYearGradeSectionStudentMapping);
    }
    
    public virtual async Task InsertAcademicYearGradeSectionStudentMappingAsync(IEnumerable<AcademicYearGradeSectionStudentMapping> academicYearGradeSectionStudentMappings)
    {
        if (academicYearGradeSectionStudentMappings == null || !academicYearGradeSectionStudentMappings.Any())
            return;

        await _academicYearGradeSectionStudentMappingRepository.InsertAsync(academicYearGradeSectionStudentMappings.ToList());
    }

    public virtual async Task UpdateAcademicYearGradeSectionStudentMappingAsync(AcademicYearGradeSectionStudentMapping academicYearGradeSectionStudentMapping)
    {
        if (academicYearGradeSectionStudentMapping == null)
            return;

        await _academicYearGradeSectionStudentMappingRepository.UpdateAsync(academicYearGradeSectionStudentMapping);
    }

    public virtual async Task UpdateAcademicYearGradeSectionStudentMappingAsync(IEnumerable<AcademicYearGradeSectionStudentMapping> academicYearGradeSectionStudentMappings)
    {
        if (academicYearGradeSectionStudentMappings == null || !academicYearGradeSectionStudentMappings.Any())
            return;

        await _academicYearGradeSectionStudentMappingRepository.UpdateAsync(academicYearGradeSectionStudentMappings.ToList());
    }

    public virtual async Task DeleteAcademicYearGradeSectionStudentMappingAsync(AcademicYearGradeSectionStudentMapping academicYearGradeSectionStudentMapping)
    {
        if (academicYearGradeSectionStudentMapping == null)
            return;

        await _academicYearGradeSectionStudentMappingRepository.DeleteAsync(academicYearGradeSectionStudentMapping);
    }

    public virtual async Task DeleteAcademicYearGradeSectionStudentMappingAsync(IEnumerable<AcademicYearGradeSectionStudentMapping> academicYearGradeSectionStudentMappings)
    {
        if (academicYearGradeSectionStudentMappings == null || !academicYearGradeSectionStudentMappings.Any())
            return;

        await _academicYearGradeSectionStudentMappingRepository.DeleteAsync(academicYearGradeSectionStudentMappings.ToList());
    }

    #endregion
}