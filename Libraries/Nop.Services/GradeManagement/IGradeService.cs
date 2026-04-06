using Nop.Core;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.GradeManagement;

namespace Nop.Services.GradeManagement;

public partial interface IGradeService
{
    Task<IPagedList<Grade>> GetAllGradesAsync(
        int id = 0, IEnumerable<int> ids = null,
        string name = null, IEnumerable<string> names = null,
        BooleanFilter isActive = BooleanFilter.True,
        BooleanFilter deleted = BooleanFilter.False,
        int storeId = 0, IEnumerable<int> storeIds = null,


        BooleanFilter limitedToStores = BooleanFilter.Both,

        int pageIndex = 0, int pageSize = int.MaxValue);

    Task<Grade> GetGradeByIdAsync(int id);

    Task<IList<Grade>> GetGradesByIdsAsync(IEnumerable<int> ids);

    Task InsertGradeAsync(Grade grade);
    
    Task InsertGradeAsync(IEnumerable<Grade> grades);

    Task UpdateGradeAsync(Grade grade);

    Task UpdateGradeAsync(IEnumerable<Grade> grades);

    Task DeleteGradeAsync(Grade grade);

    Task DeleteGradeAsync(IEnumerable<Grade> grades);
}