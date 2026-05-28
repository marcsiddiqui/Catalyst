using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Admissions;

public partial record AdmissionDocumentModel : BaseNopEntityModel
{
    #region Ctor

    public AdmissionDocumentModel()
    {

    }

    #endregion

    #region Properties

    [NopResourceDisplayName("Admin.Configuration.AdmissionDocuments.Fields.AdmissionId")]
    public int AdmissionId { get; set; }

    [NopResourceDisplayName("Admin.Configuration.AdmissionDocuments.Fields.AdmissionDocumentTypeId")]
    public int AdmissionDocumentTypeId { get; set; }

    [NopResourceDisplayName("Admin.Configuration.AdmissionDocuments.Fields.FileName")]
    public string FileName { get; set; }

    [NopResourceDisplayName("Admin.Configuration.AdmissionDocuments.Fields.FilePath")]
    public string FilePath { get; set; }

    [NopResourceDisplayName("Admin.Configuration.AdmissionDocuments.Fields.Deleted")]
    public bool Deleted { get; set; }

    [NopResourceDisplayName("Admin.Configuration.AdmissionDocuments.Fields.CreatedBy")]
    public int CreatedBy { get; set; }

    [NopResourceDisplayName("Admin.Configuration.AdmissionDocuments.Fields.CreatedOnUtc")]
    public DateTime CreatedOnUtc { get; set; }

    [NopResourceDisplayName("Admin.Configuration.AdmissionDocuments.Fields.UpdatedBy")]
    public int UpdatedBy { get; set; }

    [NopResourceDisplayName("Admin.Configuration.AdmissionDocuments.Fields.UpdatedOnUtc")]
    public DateTime? UpdatedOnUtc { get; set; }



    #endregion
}

