using Nop.Core;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Stores;
using Nop.Core.Domain.Common;

namespace Nop.Core.Domain.GradeManagement;

public partial class Grade : BaseEntity, ILocalizedEntity, IStoreMappingSupported, ISoftDeletedEntity
{
    public string Name { get; set; }

    public bool IsActive { get; set; }

    public bool Deleted { get; set; }

    public decimal BaseFeeAmount { get; set; }

    public int StoreId { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedOnUtc { get; set; }

    public int UpdatedBy { get; set; }

    public DateTime? UpdatedOnUtc { get; set; }

    public bool LimitedToStores { get; set; }


}