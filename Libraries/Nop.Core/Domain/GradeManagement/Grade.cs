using Nop.Core;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.LogInfo;
using Nop.Core.Domain.Stores;

namespace Nop.Core.Domain.GradeManagement;

public partial class Grade : LogInfoSupportedBaseEntity, ILocalizedEntity, IStoreMappingSupported, ISoftDeletedEntity
{
    public string Name { get; set; }

    public bool IsActive { get; set; }

    public bool Deleted { get; set; }

    public decimal BaseFeeAmount { get; set; }

    public int StoreId { get; set; }

    public bool LimitedToStores { get; set; }


}