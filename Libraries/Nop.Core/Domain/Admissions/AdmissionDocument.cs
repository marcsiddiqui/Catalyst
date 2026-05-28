using Nop.Core;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.LogInfo;

namespace Nop.Core.Domain.Admissions;

public partial class AdmissionDocument : LogInfoSupportedBaseEntity, ISoftDeletedEntity
{
    public int AdmissionId { get; set; }

    public int AdmissionDocumentTypeId { get; set; }

    public string FileName { get; set; }

    public string FilePath { get; set; }

    public bool Deleted { get; set; }
}