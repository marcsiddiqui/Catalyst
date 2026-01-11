using Nop.Core;

namespace Nop.Core.Domain.Fees;

public partial class FeePayments : BaseEntity
{
    public int FeeId { get; set; }

    public decimal PaidAmount { get; set; }

    public int StatusId { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedOnUtc { get; set; }

    public int UpdatedBy { get; set; }

    public DateTime? UpdatedOnUtc { get; set; }


}