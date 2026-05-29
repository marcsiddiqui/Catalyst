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

    public string AdmissionDocumentTypeName { get; set; }

    [NopResourceDisplayName("Admin.Configuration.AdmissionDocuments.Fields.FileName")]
    public string FileName { get; set; }

    [NopResourceDisplayName("Admin.Configuration.AdmissionDocuments.Fields.FilePath")]
    public string FilePath { get; set; }

    public string PreviewUrl { get; set; }

    [NopResourceDisplayName("Admin.Configuration.AdmissionDocuments.Fields.Deleted")]
    public bool Deleted { get; set; }

    #endregion
}

