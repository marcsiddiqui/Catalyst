using Nop.Core;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.LogInfo;

namespace Nop.Core.Domain.Subjects;

public partial class Subject : LogInfoSupportedBaseEntity, ILocalizedEntity, ISoftDeletedEntity
{
    public string Name { get; set; }

    public bool IsActive { get; set; }

    public bool Deleted { get; set; }
}