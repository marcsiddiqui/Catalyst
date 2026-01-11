using Nop.Core;
using Nop.Core.Domain.Localization;

namespace Nop.Core.Domain.GenericDropDowns;

public partial class GenericDropDownOption : BaseEntity, ILocalizedEntity
{
    public int EntityId { get; set; }

    public string Text { get; set; }

    public int Value { get; set; }

    public int OrderBy { get; set; }

    public string Color { get; set; }

    public bool IsSystemOption { get; set; }

    public DateTime CreatedOnUtc { get; set; }


}