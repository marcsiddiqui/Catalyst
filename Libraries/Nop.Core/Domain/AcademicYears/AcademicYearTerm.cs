using Nop.Core.Domain.Common;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.LogInfo;

namespace Nop.Core.Domain.AcademicYears;

public partial class AcademicYearTerm : LogInfoSupportedBaseEntity, ILocalizedEntity, ISoftDeletedEntity
{
    public int AcademicYearGradeSectionMappingId { get; set; }

    public int ConductanceOrder { get; set; }

    public string Name { get; set; }

    public decimal Weitage { get; set; }

    public bool Deleted { get; set; }


}