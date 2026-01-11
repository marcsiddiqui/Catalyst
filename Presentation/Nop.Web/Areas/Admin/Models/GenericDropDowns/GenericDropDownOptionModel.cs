using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.GenericDropDowns;

public partial record GenericDropDownOptionModel : BaseNopEntityModel, ILocalizedModel<GenericDropDownOptionLocalizedModel>
{
    #region Ctor

    public GenericDropDownOptionModel()
    {
        Locales = new List<GenericDropDownOptionLocalizedModel>();


    }

    #endregion

    #region Properties

    [NopResourceDisplayName("Admin.Configuration.GenericDropDownOptions.Fields.EntityId")]
    public int EntityId { get; set; }

    [NopResourceDisplayName("Admin.Configuration.GenericDropDownOptions.Fields.Text")]
    public string Text { get; set; }

    [NopResourceDisplayName("Admin.Configuration.GenericDropDownOptions.Fields.Value")]
    public int Value { get; set; }

    [NopResourceDisplayName("Admin.Configuration.GenericDropDownOptions.Fields.OrderBy")]
    public int OrderBy { get; set; }

    [NopResourceDisplayName("Admin.Configuration.GenericDropDownOptions.Fields.Color")]
    public string Color { get; set; }

    [NopResourceDisplayName("Admin.Configuration.GenericDropDownOptions.Fields.IsSystemOption")]
    public bool IsSystemOption { get; set; }

    [NopResourceDisplayName("Admin.Configuration.GenericDropDownOptions.Fields.CreatedOnUtc")]
    public DateTime CreatedOnUtc { get; set; }

    public IList<GenericDropDownOptionLocalizedModel> Locales { get; set; }



    #endregion
}

public partial record GenericDropDownOptionLocalizedModel : ILocalizedLocaleModel
{
    public int LanguageId { get; set; }

    [NopResourceDisplayName("Admin.Configuration.GenericDropDownOptions.Fields.Text")]
    public string Text { get; set; }


}