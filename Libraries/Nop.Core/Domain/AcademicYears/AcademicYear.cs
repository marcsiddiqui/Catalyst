using Nop.Core;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Common;

namespace Nop.Core.Domain.AcademicYears;

public partial class AcademicYear : BaseEntity, ILocalizedEntity, ISoftDeletedEntity
{
    public string Name { get; set; }

    public string Description { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedOnUtc { get; set; }

    public int UpdatedBy { get; set; }

    public DateTime? UpdatedOnUtc { get; set; }

    public bool Deleted { get; set; }


}