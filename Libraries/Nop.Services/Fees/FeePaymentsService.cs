using Nop.Core;
using Nop.Core.Domain.Fees;
using Nop.Data;

namespace Nop.Services.Fees;

public partial class FeePaymentsService : IFeePaymentsService
{
    #region Fields

    protected readonly IRepository<FeePayments> _feePaymentsRepository;

    #endregion

    #region Ctor

    public FeePaymentsService(
        IRepository<FeePayments> feePaymentsRepository
        )
    {
        _feePaymentsRepository = feePaymentsRepository;
    }

    #endregion

    #region Methods

    public virtual async Task<IPagedList<FeePayments>> GetAllFeePaymentsesAsync(
        int id = 0, IEnumerable<int> ids = null,
        int feeId = 0, IEnumerable<int> feeIds = null,
        int statusId = 0, IEnumerable<int> statusIds = null,



        int pageIndex = 0, int pageSize = int.MaxValue)
    {
        var productReviews = await _feePaymentsRepository.GetAllPagedAsync(async query =>
        {
            if (id > 0)
                query = query.Where(x => x.Id == id);

            if (ids != null && ids.Any())
                query = query.Where(x => ids.Contains(x.Id));

            if (feeId > 0)
                query = query.Where(x => feeId == x.FeeId);

            if (feeIds != null && feeIds.Any())
                query = query.Where(x => feeIds.Contains(x.FeeId));

            if (statusId > 0)
                query = query.Where(x => statusId == x.StatusId);

            if (statusIds != null && statusIds.Any())
                query = query.Where(x => statusIds.Contains(x.StatusId));





            return query;

        }, pageIndex, pageSize);

        return productReviews;
    }

    public virtual async Task<FeePayments> GetFeePaymentsByIdAsync(int id)
    {
        if (id <= 0)
            return null;

        return await _feePaymentsRepository.GetByIdAsync(id);
    }

    public virtual async Task<IList<FeePayments>> GetFeePaymentsesByIdsAsync(IEnumerable<int> ids)
    {
        if (ids == null || !ids.Any())
            return null;

        return await _feePaymentsRepository.GetByIdsAsync(ids.ToList());
    }

    public virtual async Task InsertFeePaymentsAsync(FeePayments feePayments)
    {
        if (feePayments == null)
            return;

        await _feePaymentsRepository.InsertAsync(feePayments);
    }
    
    public virtual async Task InsertFeePaymentsAsync(IEnumerable<FeePayments> feePaymentses)
    {
        if (feePaymentses == null || !feePaymentses.Any())
            return;

        await _feePaymentsRepository.InsertAsync(feePaymentses.ToList());
    }

    public virtual async Task UpdateFeePaymentsAsync(FeePayments feePayments)
    {
        if (feePayments == null)
            return;

        await _feePaymentsRepository.UpdateAsync(feePayments);
    }

    public virtual async Task UpdateFeePaymentsAsync(IEnumerable<FeePayments> feePaymentses)
    {
        if (feePaymentses == null || !feePaymentses.Any())
            return;

        await _feePaymentsRepository.UpdateAsync(feePaymentses.ToList());
    }

    public virtual async Task DeleteFeePaymentsAsync(FeePayments feePayments)
    {
        if (feePayments == null)
            return;

        await _feePaymentsRepository.DeleteAsync(feePayments);
    }

    public virtual async Task DeleteFeePaymentsAsync(IEnumerable<FeePayments> feePaymentses)
    {
        if (feePaymentses == null || !feePaymentses.Any())
            return;

        await _feePaymentsRepository.DeleteAsync(feePaymentses.ToList());
    }

    #endregion
}