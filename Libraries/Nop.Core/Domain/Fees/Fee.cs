using Nop.Core;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.LogInfo;

namespace Nop.Core.Domain.Fees;

public partial class Fee : LogInfoSupportedBaseEntity, ISoftDeletedEntity
{
    public int AcademicYearGradeSectionMappingId { get; set; }

    public int CustomerId { get; set; }

    public decimal Amount { get; set; }

    public decimal Discount { get; set; }

    public int FeeTypeId { get; set; }

    public DateTime FeeDate { get; set; }

    public bool Deleted { get; set; }
}