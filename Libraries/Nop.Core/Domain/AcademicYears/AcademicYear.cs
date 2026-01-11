using Nop.Core;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.LogInfo;

namespace Nop.Core.Domain.AcademicYears;

public partial class AcademicYear : LogInfoSupportedBaseEntity, ILocalizedEntity, ISoftDeletedEntity
{
    public string Name { get; set; }

    public string Description { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public bool Deleted { get; set; }


}