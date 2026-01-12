using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.AcademicYears;

public partial record AcademicYearTermModel : BaseNopEntityModel, ILocalizedModel<AcademicYearTermLocalizedModel>
{
    #region Ctor

    public AcademicYearTermModel()
    {
        Locales = new List<AcademicYearTermLocalizedModel>();


    }

    #endregion

    #region Properties

    [NopResourceDisplayName("Admin.Configuration.AcademicYearTerms.Fields.AcademicYearGradeSectionMappingId")]
    public int AcademicYearGradeSectionMappingId { get; set; }

    [NopResourceDisplayName("Admin.Configuration.AcademicYearTerms.Fields.ConductanceOrder")]
    public int ConductanceOrder { get; set; }

    [NopResourceDisplayName("Admin.Configuration.AcademicYearTerms.Fields.Name")]
    public string Name { get; set; }

    [NopResourceDisplayName("Admin.Configuration.AcademicYearTerms.Fields.Weitage")]
    public decimal Weitage { get; set; }

    [NopResourceDisplayName("Admin.Configuration.AcademicYearTerms.Fields.CreatedBy")]
    public int CreatedBy { get; set; }

    [NopResourceDisplayName("Admin.Configuration.AcademicYearTerms.Fields.CreatedOnUtc")]
    public DateTime CreatedOnUtc { get; set; }

    [NopResourceDisplayName("Admin.Configuration.AcademicYearTerms.Fields.UpdatedBy")]
    public int UpdatedBy { get; set; }

    [NopResourceDisplayName("Admin.Configuration.AcademicYearTerms.Fields.UpdatedOnUtc")]
    public DateTime? UpdatedOnUtc { get; set; }

    [NopResourceDisplayName("Admin.Configuration.AcademicYearTerms.Fields.Deleted")]
    public bool Deleted { get; set; }

    public IList<AcademicYearTermLocalizedModel> Locales { get; set; }



    #endregion
}

public partial record AcademicYearTermLocalizedModel : ILocalizedLocaleModel
{
    public int LanguageId { get; set; }

    [NopResourceDisplayName("Admin.Configuration.AcademicYearTerms.Fields.Name")]
    public string Name { get; set; }


}