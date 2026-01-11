using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.AcademicYears;

public partial record AcademicYearModel : BaseNopEntityModel, ILocalizedModel<AcademicYearLocalizedModel>
{
    #region Ctor

    public AcademicYearModel()
    {
        Locales = new List<AcademicYearLocalizedModel>();


    }

    #endregion

    #region Properties

    [NopResourceDisplayName("Admin.Configuration.AcademicYears.Fields.Name")]
    public string Name { get; set; }

    [NopResourceDisplayName("Admin.Configuration.AcademicYears.Fields.Description")]
    public string Description { get; set; }

    [NopResourceDisplayName("Admin.Configuration.AcademicYears.Fields.StartDate")]
    public DateTime StartDate { get; set; }

    [NopResourceDisplayName("Admin.Configuration.AcademicYears.Fields.EndDate")]
    public DateTime EndDate { get; set; }

    [NopResourceDisplayName("Admin.Configuration.AcademicYears.Fields.CreatedBy")]
    public int CreatedBy { get; set; }

    [NopResourceDisplayName("Admin.Configuration.AcademicYears.Fields.CreatedOnUtc")]
    public DateTime CreatedOnUtc { get; set; }

    [NopResourceDisplayName("Admin.Configuration.AcademicYears.Fields.UpdatedBy")]
    public int UpdatedBy { get; set; }

    [NopResourceDisplayName("Admin.Configuration.AcademicYears.Fields.UpdatedOnUtc")]
    public DateTime? UpdatedOnUtc { get; set; }

    [NopResourceDisplayName("Admin.Configuration.AcademicYears.Fields.Deleted")]
    public bool Deleted { get; set; }

    public IList<AcademicYearLocalizedModel> Locales { get; set; }



    #endregion
}

public partial record AcademicYearLocalizedModel : ILocalizedLocaleModel
{
    public int LanguageId { get; set; }

    [NopResourceDisplayName("Admin.Configuration.AcademicYears.Fields.Name")]
    public string Name { get; set; }


}