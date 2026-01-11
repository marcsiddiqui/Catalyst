using Nop.Core;

namespace Nop.Core.Domain.HolidaysNEvents;

public partial class AcademicYearGradeSectionEventMapping : BaseEntity
{
    public int EventId { get; set; }

    public int AcademicYearGradeSectionMappingId { get; set; }

    public decimal Amount { get; set; }


}