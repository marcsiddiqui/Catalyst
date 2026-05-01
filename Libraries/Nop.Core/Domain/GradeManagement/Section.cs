using Nop.Core.Domain.Common;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.LogInfo;

namespace Nop.Core.Domain.GradeManagement;

public partial class Section : LogInfoSupportedBaseEntity, ILocalizedEntity, ISoftDeletedEntity
{
    public string Name { get; set; }

    public bool Deleted { get; set; }
}