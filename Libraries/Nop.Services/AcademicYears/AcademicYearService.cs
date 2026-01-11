using Nop.Core;
using Nop.Core.Domain.AcademicYears;
using Nop.Data;

namespace Nop.Services.AcademicYears;

public partial class AcademicYearService : IAcademicYearService
{
    #region Fields

    protected readonly IRepository<AcademicYear> _academicYearRepository;

    #endregion

    #region Ctor

    public AcademicYearService(
        IRepository<AcademicYear> academicYearRepository
        )
    {
        _academicYearRepository = academicYearRepository;
    }

    #endregion

    #region Methods

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
}