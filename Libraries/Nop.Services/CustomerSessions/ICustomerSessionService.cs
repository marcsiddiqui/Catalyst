using Nop.Core;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.CustomerSessions;

namespace Nop.Services.CustomerSessions;

public partial interface ICustomerSessionService
{
    Task<IPagedList<CustomerSession>> GetAllCustomerSessionsAsync(
        int id = 0, IEnumerable<int> ids = null,
        int customerId = 0, IEnumerable<int> customerIds = null,
        int loginTypeId = 0, IEnumerable<int> loginTypeIds = null,
        BooleanFilter isActive = BooleanFilter.True,
        string deviceToken = null, IEnumerable<string> deviceTokens = null,
        string timeZone = null, IEnumerable<string> timeZones = null,
        string deviceVersion = null, IEnumerable<string> deviceVersions = null,

        int pageIndex = 0, int pageSize = int.MaxValue);

    Task<CustomerSession> GetCustomerSessionByIdAsync(int id);

    Task<IList<CustomerSession>> GetCustomerSessionsByIdsAsync(IEnumerable<int> ids);

    Task InsertCustomerSessionAsync(CustomerSession customerSession);
    
    Task InsertCustomerSessionAsync(IEnumerable<CustomerSession> customerSessions);

    Task UpdateCustomerSessionAsync(CustomerSession customerSession);

    Task UpdateCustomerSessionAsync(IEnumerable<CustomerSession> customerSessions);

    Task DeleteCustomerSessionAsync(CustomerSession customerSession);

    Task DeleteCustomerSessionAsync(IEnumerable<CustomerSession> customerSessions);
}