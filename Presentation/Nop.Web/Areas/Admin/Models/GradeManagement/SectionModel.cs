using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.GradeManagement;

public partial record SectionModel : BaseNopEntityModel, ILocalizedModel<SectionLocalizedModel>
{
    #region Ctor

    public SectionModel()
    {
        Locales = new List<SectionLocalizedModel>();


    }

    #endregion

    #region Properties

    [NopResourceDisplayName("Admin.Configuration.Sections.Fields.Name")]
    public string Name { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Sections.Fields.Deleted")]
    public bool Deleted { get; set; }

    public IList<SectionLocalizedModel> Locales { get; set; }



    #endregion
}

public partial record SectionLocalizedModel : ILocalizedLocaleModel
{
    public int LanguageId { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Sections.Fields.Name")]
    public string Name { get; set; }


}