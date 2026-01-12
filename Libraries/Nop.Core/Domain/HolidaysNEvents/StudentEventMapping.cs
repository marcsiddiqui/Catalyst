namespace Nop.Core.Domain.HolidaysNEvents;

public partial class StudentEventMapping : BaseEntity
{
    public int EventId { get; set; }

    public int CustomerId { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedOnUtc { get; set; }

    public int UpdatedBy { get; set; }

    public DateTime? UpdatedOnUtc { get; set; }


}