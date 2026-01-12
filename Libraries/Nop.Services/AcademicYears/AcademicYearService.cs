using Nop.Core;
using Nop.Core.Domain.AcademicYears;
using Nop.Data;

namespace Nop.Services.AcademicYears;

public partial class AcademicYearService : IAcademicYearService
{
    #region Fields

    protected readonly IRepository<AcademicYear> _academicYearRepository;
    protected readonly IRepository<AcademicYearGradeSectionMapping> _academicYearGradeSectionMappingRepository;
    protected readonly IRepository<AcademicYearTerm> _academicYearTermRepository;

    #endregion

    #region Ctor

    public AcademicYearService(
        IRepository<AcademicYear> academicYearRepository,
        IRepository<AcademicYearGradeSectionMapping> academicYearGradeSectionMappingRepository,
        IRepository<AcademicYearTerm> academicYearTermRepository
        )
    {
        _academicYearRepository = academicYearRepository;
        _academicYearGradeSectionMappingRepository = academicYearGradeSectionMappingRepository;
        _academicYearTermRepository = academicYearTermRepository;
    }

    #endregion

    #region Methods

    #region AcademicYear

    public virtual async Task<IPagedList<AcademicYear>> GetAllAcademicYearsAsync(
        int id = 0, IEnumerable<int> ids = null,
        string name = null, IEnumerable<string> names = null,


        BooleanFilter deleted = BooleanFilter.False,

        int pageIndex = 0, int pageSize = int.MaxValue)
    {
        var productReviews = await _academicYearRepository.GetAllPagedAsync(async query =>
        {
            if (id > 0)
                query = query.Where(x => x.Id == id);

            if (ids != null && ids.Any())
                query = query.Where(x => ids.Contains(x.Id));

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(x => name == x.Name);

            if (names != null && names.Any())
                query = query.Where(x => names.Contains(x.Name));



            query = query.WhereBoolean(x => x.Deleted, deleted);



            return query;

        }, pageIndex, pageSize);

        return productReviews;
    }

    public virtual async Task<AcademicYear> GetAcademicYearByIdAsync(int id)
    {
        if (id <= 0)
            return null;

        return await _academicYearRepository.GetByIdAsync(id);
    }

    public virtual async Task<IList<AcademicYear>> GetAcademicYearsByIdsAsync(IEnumerable<int> ids)
    {
        if (ids == null || !ids.Any())
            return null;

        return await _academicYearRepository.GetByIdsAsync(ids.ToList());
    }

    public virtual async Task InsertAcademicYearAsync(AcademicYear academicYear)
    {
        if (academicYear == null)
            return;

        await _academicYearRepository.InsertAsync(academicYear);
    }
    
    public virtual async Task InsertAcademicYearAsync(IEnumerable<AcademicYear> academicYears)
    {
        if (academicYears == null || !academicYears.Any())
            return;

        await _academicYearRepository.InsertAsync(academicYears.ToList());
    }

    public virtual async Task UpdateAcademicYearAsync(AcademicYear academicYear)
    {
        if (academicYear == null)
            return;

        await _academicYearRepository.UpdateAsync(academicYear);
    }

    public virtual async Task UpdateAcademicYearAsync(IEnumerable<AcademicYear> academicYears)
    {
        if (academicYears == null || !academicYears.Any())
            return;

        await _academicYearRepository.UpdateAsync(academicYears.ToList());
    }

    public virtual async Task DeleteAcademicYearAsync(AcademicYear academicYear)
    {
        if (academicYear == null)
            return;

        await _academicYearRepository.DeleteAsync(academicYear);
    }

    public virtual async Task DeleteAcademicYearAsync(IEnumerable<AcademicYear> academicYears)
    {
        if (academicYears == null || !academicYears.Any())
            return;

        await _academicYearRepository.DeleteAsync(academicYears.ToList());
    }

    #endregion

    #region AcademicYearGradeSectionMapping

    public virtual async Task<IPagedList<AcademicYearGradeSectionMapping>> GetAllAcademicYearGradeSectionMappingsAsync(
        int id = 0, IEnumerable<int> ids = null,
        int academicYearId = 0, IEnumerable<int> academicYearIds = null,
        int gradeId = 0, IEnumerable<int> gradeIds = null,
        int sectionId = 0, IEnumerable<int> sectionIds = null,


        int pageIndex = 0, int pageSize = int.MaxValue)
    {
        var productReviews = await _academicYearGradeSectionMappingRepository.GetAllPagedAsync(async query =>
        {
            if (id > 0)
                query = query.Where(x => x.Id == id);

            if (ids != null && ids.Any())
                query = query.Where(x => ids.Contains(x.Id));

            if (academicYearId > 0)
                query = query.Where(x => academicYearId == x.AcademicYearId);

            if (academicYearIds != null && academicYearIds.Any())
                query = query.Where(x => academicYearIds.Contains(x.AcademicYearId));

            if (gradeId > 0)
                query = query.Where(x => gradeId == x.GradeId);

            if (gradeIds != null && gradeIds.Any())
                query = query.Where(x => gradeIds.Contains(x.GradeId));

            if (sectionId > 0)
                query = query.Where(x => sectionId == x.SectionId);

            if (sectionIds != null && sectionIds.Any())
                query = query.Where(x => sectionIds.Contains(x.SectionId));




            return query;

        }, pageIndex, pageSize);

        return productReviews;
    }

    public virtual async Task<AcademicYearGradeSectionMapping> GetAcademicYearGradeSectionMappingByIdAsync(int id)
    {
        if (id <= 0)
            return null;

        return await _academicYearGradeSectionMappingRepository.GetByIdAsync(id);
    }

    public virtual async Task<IList<AcademicYearGradeSectionMapping>> GetAcademicYearGradeSectionMappingsByIdsAsync(IEnumerable<int> ids)
    {
        if (ids == null || !ids.Any())
            return null;

        return await _academicYearGradeSectionMappingRepository.GetByIdsAsync(ids.ToList());
    }

    public virtual async Task InsertAcademicYearGradeSectionMappingAsync(AcademicYearGradeSectionMapping academicYearGradeSectionMapping)
    {
        if (academicYearGradeSectionMapping == null)
            return;

        await _academicYearGradeSectionMappingRepository.InsertAsync(academicYearGradeSectionMapping);
    }

    public virtual async Task InsertAcademicYearGradeSectionMappingAsync(IEnumerable<AcademicYearGradeSectionMapping> academicYearGradeSectionMappings)
    {
        if (academicYearGradeSectionMappings == null || !academicYearGradeSectionMappings.Any())
            return;

        await _academicYearGradeSectionMappingRepository.InsertAsync(academicYearGradeSectionMappings.ToList());
    }

    public virtual async Task UpdateAcademicYearGradeSectionMappingAsync(AcademicYearGradeSectionMapping academicYearGradeSectionMapping)
    {
        if (academicYearGradeSectionMapping == null)
            return;

        await _academicYearGradeSectionMappingRepository.UpdateAsync(academicYearGradeSectionMapping);
    }

    public virtual async Task UpdateAcademicYearGradeSectionMappingAsync(IEnumerable<AcademicYearGradeSectionMapping> academicYearGradeSectionMappings)
    {
        if (academicYearGradeSectionMappings == null || !academicYearGradeSectionMappings.Any())
            return;

        await _academicYearGradeSectionMappingRepository.UpdateAsync(academicYearGradeSectionMappings.ToList());
    }

    public virtual async Task DeleteAcademicYearGradeSectionMappingAsync(AcademicYearGradeSectionMapping academicYearGradeSectionMapping)
    {
        if (academicYearGradeSectionMapping == null)
            return;

        await _academicYearGradeSectionMappingRepository.DeleteAsync(academicYearGradeSectionMapping);
    }

    public virtual async Task DeleteAcademicYearGradeSectionMappingAsync(IEnumerable<AcademicYearGradeSectionMapping> academicYearGradeSectionMappings)
    {
        if (academicYearGradeSectionMappings == null || !academicYearGradeSectionMappings.Any())
            return;

        await _academicYearGradeSectionMappingRepository.DeleteAsync(academicYearGradeSectionMappings.ToList());
    }

    #endregion

    #region AcademicYearTerm

    public virtual async Task<IPagedList<AcademicYearTerm>> GetAllAcademicYearTermsAsync(
        int id = 0, IEnumerable<int> ids = null,
        int academicYearGradeSectionMappingId = 0, IEnumerable<int> academicYearGradeSectionMappingIds = null,

        string name = null, IEnumerable<string> names = null,


        BooleanFilter deleted = BooleanFilter.False,

        int pageIndex = 0, int pageSize = int.MaxValue)
    {
        var productReviews = await _academicYearTermRepository.GetAllPagedAsync(async query =>
        {
            if (id > 0)
                query = query.Where(x => x.Id == id);

            if (ids != null && ids.Any())
                query = query.Where(x => ids.Contains(x.Id));

            if (academicYearGradeSectionMappingId > 0)
                query = query.Where(x => academicYearGradeSectionMappingId == x.AcademicYearGradeSectionMappingId);

            if (academicYearGradeSectionMappingIds != null && academicYearGradeSectionMappingIds.Any())
                query = query.Where(x => academicYearGradeSectionMappingIds.Contains(x.AcademicYearGradeSectionMappingId));


            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(x => name == x.Name);

            if (names != null && names.Any())
                query = query.Where(x => names.Contains(x.Name));



            query = query.WhereBoolean(x => x.Deleted, deleted);



            return query;

        }, pageIndex, pageSize);

        return productReviews;
    }

    public virtual async Task<AcademicYearTerm> GetAcademicYearTermByIdAsync(int id)
    {
        if (id <= 0)
            return null;

        return await _academicYearTermRepository.GetByIdAsync(id);
    }

    public virtual async Task<IList<AcademicYearTerm>> GetAcademicYearTermsByIdsAsync(IEnumerable<int> ids)
    {
        if (ids == null || !ids.Any())
            return null;

        return await _academicYearTermRepository.GetByIdsAsync(ids.ToList());
    }

    public virtual async Task InsertAcademicYearTermAsync(AcademicYearTerm academicYearTerm)
    {
        if (academicYearTerm == null)
            return;

        await _academicYearTermRepository.InsertAsync(academicYearTerm);
    }

    public virtual async Task InsertAcademicYearTermAsync(IEnumerable<AcademicYearTerm> academicYearTerms)
    {
        if (academicYearTerms == null || !academicYearTerms.Any())
            return;

        await _academicYearTermRepository.InsertAsync(academicYearTerms.ToList());
    }

    public virtual async Task UpdateAcademicYearTermAsync(AcademicYearTerm academicYearTerm)
    {
        if (academicYearTerm == null)
            return;

        await _academicYearTermRepository.UpdateAsync(academicYearTerm);
    }

    public virtual async Task UpdateAcademicYearTermAsync(IEnumerable<AcademicYearTerm> academicYearTerms)
    {
        if (academicYearTerms == null || !academicYearTerms.Any())
            return;

        await _academicYearTermRepository.UpdateAsync(academicYearTerms.ToList());
    }

    public virtual async Task DeleteAcademicYearTermAsync(AcademicYearTerm academicYearTerm)
    {
        if (academicYearTerm == null)
            return;

        await _academicYearTermRepository.DeleteAsync(academicYearTerm);
    }

    public virtual async Task DeleteAcademicYearTermAsync(IEnumerable<AcademicYearTerm> academicYearTerms)
    {
        if (academicYearTerms == null || !academicYearTerms.Any())
            return;

        await _academicYearTermRepository.DeleteAsync(academicYearTerms.ToList());
    }


    #endregion

    #endregion
}