namespace Nop.Core.Domain.LogInfo;

/// <summary>
/// Represents an entity which supports LogInfo columns
/// </summary>
public partial class LogInfoSupportedBaseEntity : BaseEntity
{
    public int CreatedBy { get; set; }

    public DateTime CreatedOnUtc { get; set; }

    public int UpdatedBy { get; set; }

    public DateTime? UpdatedOnUtc { get; set; }
}