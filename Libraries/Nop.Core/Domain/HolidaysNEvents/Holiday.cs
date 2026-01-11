using Nop.Core;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Stores;
using Nop.Core.Domain.Common;

namespace Nop.Core.Domain.HolidaysNEvents;

public partial class Holiday : BaseEntity, ILocalizedEntity, IStoreMappingSupported, ISoftDeletedEntity
{
    public int AcademicYearId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public DateTime DateFromUtc { get; set; }

    public DateTime DateToUtc { get; set; }

    public int StoreId { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedOnUtc { get; set; }

    public int UpdatedBy { get; set; }

    public DateTime? UpdatedOnUtc { get; set; }

    public bool Deleted { get; set; }

    public bool LimitedToStores { get; set; }


}