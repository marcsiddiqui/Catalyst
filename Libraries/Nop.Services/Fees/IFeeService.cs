using Nop.Core;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Fees;

namespace Nop.Services.Fees;

public partial interface IFeeService
{
    Task<IPagedList<Fee>> GetAllFeesAsync(
        int id = 0, IEnumerable<int> ids = null,
        int academicYearGradeSectionMappingId = 0, IEnumerable<int> academicYearGradeSectionMappingIds = null,
        int customerId = 0, IEnumerable<int> customerIds = null,
        int feeTypeId = 0, IEnumerable<int> feeTypeIds = null,
        BooleanFilter deleted = BooleanFilter.False,



        int pageIndex = 0, int pageSize = int.MaxValue);

    Task<Fee> GetFeeByIdAsync(int id);

    Task<IList<Fee>> GetFeesByIdsAsync(IEnumerable<int> ids);

    Task InsertFeeAsync(Fee fee);
    
    Task InsertFeeAsync(IEnumerable<Fee> fees);

    Task UpdateFeeAsync(Fee fee);

    Task UpdateFeeAsync(IEnumerable<Fee> fees);

    Task DeleteFeeAsync(Fee fee);

    Task DeleteFeeAsync(IEnumerable<Fee> fees);
}