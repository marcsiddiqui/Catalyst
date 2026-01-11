using Nop.Core;
using Nop.Core.Domain.LogInfo;

namespace Nop.Core.Domain.Fees;

public partial class FeePayment : LogInfoSupportedBaseEntity
{
    public int FeeId { get; set; }

    public decimal PaidAmount { get; set; }

    public int StatusId { get; set; }
}