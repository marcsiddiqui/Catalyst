using Nop.Core;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.LogInfo;
using Nop.Core.Domain.Stores;

namespace Nop.Core.Domain.HolidaysNEvents;

public partial class Holiday : LogInfoSupportedBaseEntity, ILocalizedEntity, IStoreMappingSupported, ISoftDeletedEntity
{
    public int AcademicYearId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public DateTime DateFromUtc { get; set; }

    public DateTime DateToUtc { get; set; }

    public int StoreId { get; set; }

    public bool Deleted { get; set; }

    public bool LimitedToStores { get; set; }


}