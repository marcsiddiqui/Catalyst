using Nop.Core;

namespace Nop.Core.Domain.CustomerSessions;

public partial class CustomerSession : BaseEntity
{
    public Guid SessionId { get; set; }

    public int CustomerId { get; set; }

    public int LoginTypeId { get; set; }

    public bool IsActive { get; set; }

    public string DeviceToken { get; set; }

    public string TimeZone { get; set; }

    public string DeviceVersion { get; set; }

    public DateTime CreatedOnUtc { get; set; }

    public DateTime? ExpiresOnUtc { get; set; }

    public DateTime? LastActivityOnUtc { get; set; }


}