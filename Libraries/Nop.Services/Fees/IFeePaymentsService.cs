using Nop.Core;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Fees;

namespace Nop.Services.Fees;

public partial interface IFeePaymentsService
{
    Task<IPagedList<FeePayments>> GetAllFeePaymentsesAsync(
        int id = 0, IEnumerable<int> ids = null,
        int feeId = 0, IEnumerable<int> feeIds = null,
        int statusId = 0, IEnumerable<int> statusIds = null,



        int pageIndex = 0, int pageSize = int.MaxValue);

    Task<FeePayments> GetFeePaymentsByIdAsync(int id);

    Task<IList<FeePayments>> GetFeePaymentsesByIdsAsync(IEnumerable<int> ids);

    Task InsertFeePaymentsAsync(FeePayments feePayments);
    
    Task InsertFeePaymentsAsync(IEnumerable<FeePayments> feePaymentses);

    Task UpdateFeePaymentsAsync(FeePayments feePayments);

    Task UpdateFeePaymentsAsync(IEnumerable<FeePayments> feePaymentses);

    Task DeleteFeePaymentsAsync(FeePayments feePayments);

    Task DeleteFeePaymentsAsync(IEnumerable<FeePayments> feePaymentses);
}