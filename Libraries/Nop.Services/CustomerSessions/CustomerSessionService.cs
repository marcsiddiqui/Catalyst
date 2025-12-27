using Nop.Core;
using Nop.Core.Domain.CustomerSessions;
using Nop.Data;

namespace Nop.Services.CustomerSessions;

public partial class CustomerSessionService : ICustomerSessionService
{
    #region Fields

    protected readonly IRepository<CustomerSession> _customerSessionRepository;

    #endregion

    #region Ctor

    public CustomerSessionService(
        IRepository<CustomerSession> customerSessionRepository
        )
    {
        _customerSessionRepository = customerSessionRepository;
    }

    #endregion

    #region Methods

    public virtual async Task<IPagedList<CustomerSession>> GetAllCustomerSessionsAsync(
        int id = 0, IEnumerable<int> ids = null,
        int customerId = 0, IEnumerable<int> customerIds = null,
        int loginTypeId = 0, IEnumerable<int> loginTypeIds = null,
        BooleanFilter isActive = BooleanFilter.True,
        string deviceToken = null, IEnumerable<string> deviceTokens = null,
        string timeZone = null, IEnumerable<string> timeZones = null,
        string deviceVersion = null, IEnumerable<string> deviceVersions = null,

        int pageIndex = 0, int pageSize = int.MaxValue)
    {
        var productReviews = await _customerSessionRepository.GetAllPagedAsync(async query =>
        {
            if (id > 0)
                query = query.Where(x => x.Id == id);

            if (ids != null && ids.Any())
                query = query.Where(x => ids.Contains(x.Id));

            if (customerId > 0)
                query = query.Where(x => customerId == x.CustomerId);

            if (customerIds != null && customerIds.Any())
                query = query.Where(x => customerIds.Contains(x.CustomerId));

            if (loginTypeId > 0)
                query = query.Where(x => loginTypeId == x.LoginTypeId);

            if (loginTypeIds != null && loginTypeIds.Any())
                query = query.Where(x => loginTypeIds.Contains(x.LoginTypeId));

            query = query.WhereBoolean(x => x.IsActive, isActive);

            if (!string.IsNullOrWhiteSpace(deviceToken))
                query = query.Where(x => deviceToken == x.DeviceToken);

            if (deviceTokens != null && deviceTokens.Any())
                query = query.Where(x => deviceTokens.Contains(x.DeviceToken));

            if (!string.IsNullOrWhiteSpace(timeZone))
                query = query.Where(x => timeZone == x.TimeZone);

            if (timeZones != null && timeZones.Any())
                query = query.Where(x => timeZones.Contains(x.TimeZone));

            if (!string.IsNullOrWhiteSpace(deviceVersion))
                query = query.Where(x => deviceVersion == x.DeviceVersion);

            if (deviceVersions != null && deviceVersions.Any())
                query = query.Where(x => deviceVersions.Contains(x.DeviceVersion));

            return query;

        }, pageIndex, pageSize);

        return productReviews;
    }

    public virtual async Task<CustomerSession> GetCustomerSessionByIdAsync(int id)
    {
        if (id <= 0)
            return null;

        return await _customerSessionRepository.GetByIdAsync(id);
    }

    public virtual async Task<IList<CustomerSession>> GetCustomerSessionsByIdsAsync(IEnumerable<int> ids)
    {
        if (ids == null || !ids.Any())
            return null;

        return await _customerSessionRepository.GetByIdsAsync(ids.ToList());
    }

    public virtual async Task InsertCustomerSessionAsync(CustomerSession customerSession)
    {
        if (customerSession == null)
            return;

        await _customerSessionRepository.InsertAsync(customerSession);
    }
    
    public virtual async Task InsertCustomerSessionAsync(IEnumerable<CustomerSession> customerSessions)
    {
        if (customerSessions == null || !customerSessions.Any())
            return;

        await _customerSessionRepository.InsertAsync(customerSessions.ToList());
    }

    public virtual async Task UpdateCustomerSessionAsync(CustomerSession customerSession)
    {
        if (customerSession == null)
            return;

        await _customerSessionRepository.UpdateAsync(customerSession);
    }

    public virtual async Task UpdateCustomerSessionAsync(IEnumerable<CustomerSession> customerSessions)
    {
        if (customerSessions == null || !customerSessions.Any())
            return;

        await _customerSessionRepository.UpdateAsync(customerSessions.ToList());
    }

    public virtual async Task DeleteCustomerSessionAsync(CustomerSession customerSession)
    {
        if (customerSession == null)
            return;

        await _customerSessionRepository.DeleteAsync(customerSession);
    }

    public virtual async Task DeleteCustomerSessionAsync(IEnumerable<CustomerSession> customerSessions)
    {
        if (customerSessions == null || !customerSessions.Any())
            return;

        await _customerSessionRepository.DeleteAsync(customerSessions.ToList());
    }

    #endregion
}