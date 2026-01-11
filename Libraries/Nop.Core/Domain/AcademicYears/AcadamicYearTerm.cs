using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Common;

namespace Nop.Core.Domain.AcademicYears;

public partial class AcadamicYearTerm : BaseEntity, ILocalizedEntity, ISoftDeletedEntity
{
    public int AcademicYearGradeSectionMappingId { get; set; }

    public int ConductanceOrder { get; set; }

    public string Name { get; set; }

    public decimal Weitage { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedOnUtc { get; set; }

    public int UpdatedBy { get; set; }

    public DateTime? UpdatedOnUtc { get; set; }

    public bool Deleted { get; set; }


}