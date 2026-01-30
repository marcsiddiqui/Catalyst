using Nop.Core;
using Nop.Core.Domain.Fees;
using Nop.Data;
using static Nop.Services.Security.StandardPermission;

namespace Nop.Services.Fees;

public partial class FeeService : IFeeService
{
    #region Fields

    protected readonly IRepository<Fee> _feeRepository;
    private readonly IWorkContext _workContext;

    #endregion

    #region Ctor

    public FeeService(
        IRepository<Fee> feeRepository,
        IWorkContext workContext
        )
    {
        _feeRepository = feeRepository;
        _workContext = workContext;
    }

    #endregion

    #region Methods

    public virtual async Task<IPagedList<Fee>> GetAllFeesAsync(
        int id = 0, IEnumerable<int> ids = null,
        int academicYearGradeSectionMappingId = 0, IEnumerable<int> academicYearGradeSectionMappingIds = null,
        int customerId = 0, IEnumerable<int> customerIds = null,
        int feeTypeId = 0, IEnumerable<int> feeTypeIds = null,
        BooleanFilter deleted = BooleanFilter.False,



        int pageIndex = 0, int pageSize = int.MaxValue)
    {
        var productReviews = await _feeRepository.GetAllPagedAsync(async query =>
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

            if (feeTypeId > 0)
                query = query.Where(x => feeTypeId == x.FeeTypeId);

            if (feeTypeIds != null && feeTypeIds.Any())
                query = query.Where(x => feeTypeIds.Contains(x.FeeTypeId));

            query = query.WhereBoolean(x => x.Deleted, deleted);





            return query;

        }, pageIndex, pageSize);

        return productReviews;
    }

    public virtual async Task<Fee> GetFeeByIdAsync(int id)
    {
        if (id <= 0)
            return null;

        return await _feeRepository.GetByIdAsync(id);
    }

    public virtual async Task<IList<Fee>> GetFeesByIdsAsync(IEnumerable<int> ids)
    {
        if (ids == null || !ids.Any())
            return null;

        return await _feeRepository.GetByIdsAsync(ids.ToList());
    }

    public virtual async Task InsertFeeAsync(Fee fee)
    {
        if (fee == null)
            return;

        fee.CreatedBy = (await _workContext.GetCurrentCustomerAsync()).Id;
        fee.CreatedOnUtc = DateTime.UtcNow;

        await _feeRepository.InsertAsync(fee);
    }
    
    public virtual async Task InsertFeeAsync(IEnumerable<Fee> fees)
    {
        if (fees == null || !fees.Any())
            return;

        var customerId = (await _workContext.GetCurrentCustomerAsync()).Id;
        foreach (var fee in fees)
        {
            fee.CreatedBy = customerId;
            fee.CreatedOnUtc = DateTime.UtcNow;
        }

        await _feeRepository.InsertAsync(fees.ToList());
    }

    public virtual async Task UpdateFeeAsync(Fee fee)
    {
        if (fee == null)
            return;

        fee.UpdatedBy = (await _workContext.GetCurrentCustomerAsync()).Id;
        fee.UpdatedOnUtc = DateTime.UtcNow;

        await _feeRepository.UpdateAsync(fee);
    }

    public virtual async Task UpdateFeeAsync(IEnumerable<Fee> fees)
    {
        if (fees == null || !fees.Any())
            return;

        var customerId = (await _workContext.GetCurrentCustomerAsync()).Id;
        foreach (var fee in fees)
        {
            fee.UpdatedBy = customerId;
            fee.UpdatedOnUtc = DateTime.UtcNow;
        }

        await _feeRepository.UpdateAsync(fees.ToList());
    }

    public virtual async Task DeleteFeeAsync(Fee fee)
    {
        if (fee == null)
            return;

        await _feeRepository.DeleteAsync(fee);
    }

    public virtual async Task DeleteFeeAsync(IEnumerable<Fee> fees)
    {
        if (fees == null || !fees.Any())
            return;

        await _feeRepository.DeleteAsync(fees.ToList());
    }

    #endregion
}