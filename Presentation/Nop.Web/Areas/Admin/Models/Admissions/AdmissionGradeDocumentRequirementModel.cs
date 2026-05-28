using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Admissions;

public partial record AdmissionGradeDocumentRequirementModel : BaseNopEntityModel
{
    #region Ctor

    public AdmissionGradeDocumentRequirementModel()
    {
        AvailableAdmissionDocumentTypes = new List<SelectListItem>();
        SelectedAdmissionDocumentTypeIds = new List<int>();
    }

    #endregion

    #region Properties

    [NopResourceDisplayName("Admin.Configuration.AdmissionGradeDocumentRequirements.Fields.GradeId")]
    public int GradeId { get; set; }

    [NopResourceDisplayName("Admin.Configuration.AdmissionGradeDocumentRequirements.Fields.AdmissionDocumentTypeId")]
    public int AdmissionDocumentTypeId { get; set; }

    public string AdmissionDocumentTypeName { get; set; }

    [NopResourceDisplayName("Admin.Configuration.AdmissionGradeDocumentRequirements.Fields.IsRequired")]
    public bool IsRequired { get; set; }

    [NopResourceDisplayName("Admin.Configuration.AdmissionGradeDocumentRequirements.Fields.Deleted")]
    public bool Deleted { get; set; }

    [NopResourceDisplayName("Admin.Configuration.AdmissionGradeDocumentRequirements.Fields.CreatedBy")]
    public int CreatedBy { get; set; }

    [NopResourceDisplayName("Admin.Configuration.AdmissionGradeDocumentRequirements.Fields.CreatedOnUtc")]
    public DateTime CreatedOnUtc { get; set; }

    [NopResourceDisplayName("Admin.Configuration.AdmissionGradeDocumentRequirements.Fields.UpdatedBy")]
    public int UpdatedBy { get; set; }

    [NopResourceDisplayName("Admin.Configuration.AdmissionGradeDocumentRequirements.Fields.UpdatedOnUtc")]
    public DateTime? UpdatedOnUtc { get; set; }

    [NopResourceDisplayName("Admin.Configuration.AdmissionGradeDocumentRequirements.Fields.AdmissionDocumentTypeId")]
    public IList<int> SelectedAdmissionDocumentTypeIds { get; set; }

    public IList<SelectListItem> AvailableAdmissionDocumentTypes { get; set; }


    #endregion
}

