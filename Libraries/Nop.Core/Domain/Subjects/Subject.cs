using Nop.Core;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Common;

namespace Nop.Core.Domain.Subjects;

public partial class Subject : BaseEntity, ILocalizedEntity, ISoftDeletedEntity
{
    public string Name { get; set; }

    public bool IsActive { get; set; }

    public bool Deleted { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedOnUtc { get; set; }

    public int UpdatedBy { get; set; }

    public DateTime? UpdatedOnUtc { get; set; }


}