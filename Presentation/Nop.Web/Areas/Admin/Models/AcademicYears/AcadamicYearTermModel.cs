using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.AcademicYears;

public partial record AcadamicYearTermModel : BaseNopEntityModel, ILocalizedModel<AcadamicYearTermLocalizedModel>
{
    #region Ctor

    public AcadamicYearTermModel()
    {
        Locales = new List<AcadamicYearTermLocalizedModel>();


    }

    #endregion

    #region Properties

    [NopResourceDisplayName("Admin.Configuration.AcadamicYearTerms.Fields.AcademicYearGradeSectionMappingId")]
    public int AcademicYearGradeSectionMappingId { get; set; }

    [NopResourceDisplayName("Admin.Configuration.AcadamicYearTerms.Fields.ConductanceOrder")]
    public int ConductanceOrder { get; set; }

    [NopResourceDisplayName("Admin.Configuration.AcadamicYearTerms.Fields.Name")]
    public string Name { get; set; }

    [NopResourceDisplayName("Admin.Configuration.AcadamicYearTerms.Fields.Weitage")]
    public decimal Weitage { get; set; }

    [NopResourceDisplayName("Admin.Configuration.AcadamicYearTerms.Fields.CreatedBy")]
    public int CreatedBy { get; set; }

    [NopResourceDisplayName("Admin.Configuration.AcadamicYearTerms.Fields.CreatedOnUtc")]
    public DateTime CreatedOnUtc { get; set; }

    [NopResourceDisplayName("Admin.Configuration.AcadamicYearTerms.Fields.UpdatedBy")]
    public int UpdatedBy { get; set; }

    [NopResourceDisplayName("Admin.Configuration.AcadamicYearTerms.Fields.UpdatedOnUtc")]
    public DateTime? UpdatedOnUtc { get; set; }

    [NopResourceDisplayName("Admin.Configuration.AcadamicYearTerms.Fields.Deleted")]
    public bool Deleted { get; set; }

    public IList<AcadamicYearTermLocalizedModel> Locales { get; set; }



    #endregion
}

public partial record AcadamicYearTermLocalizedModel : ILocalizedLocaleModel
{
    public int LanguageId { get; set; }

    [NopResourceDisplayName("Admin.Configuration.AcadamicYearTerms.Fields.Name")]
    public string Name { get; set; }


}