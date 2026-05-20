using Microsoft.AspNetCore.Mvc.Rendering;

namespace Nop.Web.Areas.Admin.Models.Common;

public partial record SearchableSelectModel
{
    public string FieldName { get; set; }

    public string FieldId { get; set; }

    public string Placeholder { get; set; }

    public IEnumerable<SelectListItem> Items { get; set; } = new List<SelectListItem>();

    public string SelectedValue { get; set; }

    public string SaveUrl { get; set; }

    public string DeleteUrl { get; set; }

    public string Category { get; set; }

    public bool? AllowAddNew { get; set; }

    public string AddText { get; set; } = "Add new";

    public string NoMatchesText { get; set; } = "No matches found";
}
