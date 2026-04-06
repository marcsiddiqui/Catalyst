using Nop.Core;
using Nop.Core.Domain.GradeManagement;
using Nop.Data;

namespace Nop.Services.GradeManagement;

public partial class GradeService : IGradeService
{
    #region Fields

    protected readonly IRepository<Grade> _gradeRepository;

    #endregion

    #region Ctor

    public GradeService(
        IRepository<Grade> gradeRepository
        )
    {
        _gradeRepository = gradeRepository;
    }

    #endregion

    #region Methods

    public virtual async Task<IPagedList<Grade>> GetAllGradesAsync(
        int id = 0, IEnumerable<int> ids = null,
        string name = null, IEnumerable<string> names = null,
        BooleanFilter isActive = BooleanFilter.True,
        BooleanFilter deleted = BooleanFilter.False,
        int storeId = 0, IEnumerable<int> storeIds = null,


        BooleanFilter limitedToStores = BooleanFilter.Both,

        int pageIndex = 0, int pageSize = int.MaxValue)
    {
        var productReviews = await _gradeRepository.GetAllPagedAsync(async query =>
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

            if (storeId > 0)
                query = query.Where(x => storeId == x.StoreId);

            if (storeIds != null && storeIds.Any())
                query = query.Where(x => storeIds.Contains(x.StoreId));



            query = query.WhereBoolean(x => x.LimitedToStores, limitedToStores);



            return query;

        }, pageIndex, pageSize);

        return productReviews;
    }

    public virtual async Task<Grade> GetGradeByIdAsync(int id)
    {
        if (id <= 0)
            return null;

        return await _gradeRepository.GetByIdAsync(id);
    }

    public virtual async Task<IList<Grade>> GetGradesByIdsAsync(IEnumerable<int> ids)
    {
        if (ids == null || !ids.Any())
            return null;

        return await _gradeRepository.GetByIdsAsync(ids.ToList());
    }

    public virtual async Task InsertGradeAsync(Grade grade)
    {
        if (grade == null)
            return;

        await _gradeRepository.InsertAsync(grade);
    }
    
    public virtual async Task InsertGradeAsync(IEnumerable<Grade> grades)
    {
        if (grades == null || !grades.Any())
            return;

        await _gradeRepository.InsertAsync(grades.ToList());
    }

    public virtual async Task UpdateGradeAsync(Grade grade)
    {
        if (grade == null)
            return;

        await _gradeRepository.UpdateAsync(grade);
    }

    public virtual async Task UpdateGradeAsync(IEnumerable<Grade> grades)
    {
        if (grades == null || !grades.Any())
            return;

        await _gradeRepository.UpdateAsync(grades.ToList());
    }

    public virtual async Task DeleteGradeAsync(Grade grade)
    {
        if (grade == null)
            return;

        await _gradeRepository.DeleteAsync(grade);
    }

    public virtual async Task DeleteGradeAsync(IEnumerable<Grade> grades)
    {
        if (grades == null || !grades.Any())
            return;

        await _gradeRepository.DeleteAsync(grades.ToList());
    }

    #endregion
}