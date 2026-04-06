using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Subjects;

public partial record SubjectModel : BaseNopEntityModel, ILocalizedModel<SubjectLocalizedModel>
{
    #region Ctor

    public SubjectModel()
    {
        Locales = new List<SubjectLocalizedModel>();


    }

    #endregion

    #region Properties

    [NopResourceDisplayName("Admin.Configuration.Subjects.Fields.Name")]
    public string Name { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Subjects.Fields.IsActive")]
    public bool IsActive { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Subjects.Fields.Deleted")]
    public bool Deleted { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Subjects.Fields.CreatedBy")]
    public int CreatedBy { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Subjects.Fields.CreatedOnUtc")]
    public DateTime CreatedOnUtc { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Subjects.Fields.UpdatedBy")]
    public int UpdatedBy { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Subjects.Fields.UpdatedOnUtc")]
    public DateTime? UpdatedOnUtc { get; set; }

    public IList<SubjectLocalizedModel> Locales { get; set; }



    #endregion
}

public partial record SubjectLocalizedModel : ILocalizedLocaleModel
{
    public int LanguageId { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Subjects.Fields.Name")]
    public string Name { get; set; }


}