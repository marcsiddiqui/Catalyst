using Nop.Core;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.LogInfo;

namespace Nop.Core.Domain.Admissions;

public partial class AdmissionGradeDocumentRequirement : LogInfoSupportedBaseEntity, ISoftDeletedEntity
{
    public int GradeId { get; set; }

    public int AdmissionDocumentTypeId { get; set; }

    public bool IsRequired { get; set; }

    public bool Deleted { get; set; }
}