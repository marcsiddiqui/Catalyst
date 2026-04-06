using Nop.Core;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Fees;

namespace Nop.Services.Fees;

public partial interface IFeePaymentsService
{
    Task<IPagedList<FeePayment>> GetAllFeePaymentsesAsync(
        int id = 0, IEnumerable<int> ids = null,
        int feeId = 0, IEnumerable<int> feeIds = null,
        int statusId = 0, IEnumerable<int> statusIds = null,



        int pageIndex = 0, int pageSize = int.MaxValue);

    Task<FeePayment> GetFeePaymentsByIdAsync(int id);

    Task<IList<FeePayment>> GetFeePaymentsesByIdsAsync(IEnumerable<int> ids);

    Task InsertFeePaymentsAsync(FeePayment feePayments);
    
    Task InsertFeePaymentsAsync(IEnumerable<FeePayment> feePaymentses);

    Task UpdateFeePaymentsAsync(FeePayment feePayments);

    Task UpdateFeePaymentsAsync(IEnumerable<FeePayment> feePaymentses);

    Task DeleteFeePaymentsAsync(FeePayment feePayments);

    Task DeleteFeePaymentsAsync(IEnumerable<FeePayment> feePaymentses);
}