using Nop.Core;
using Nop.Core.Domain.Common;

namespace Nop.Core.Domain.Fees;

public partial class Fee : BaseEntity, ISoftDeletedEntity
{
    public int AcademicYearGradeSectionMappingId { get; set; }

    public int CustomerId { get; set; }

    public decimal Amount { get; set; }

    public decimal Discount { get; set; }

    public int FeeTypeId { get; set; }

    public DateTime FeeDate { get; set; }

    public bool Deleted { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedOnUtc { get; set; }

    public int UpdatedBy { get; set; }

    public DateTime? UpdatedOnUtc { get; set; }


}