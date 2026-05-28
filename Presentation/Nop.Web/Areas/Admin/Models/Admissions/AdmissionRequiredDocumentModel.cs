using Nop.Web.Framework.Models;

namespace Nop.Web.Areas.Admin.Models.Admissions;

public partial record AdmissionRequiredDocumentModel : BaseNopModel
{
    public int AdmissionDocumentTypeId { get; set; }

    public string AdmissionDocumentTypeName { get; set; }

    public bool IsRequired { get; set; }

    public int AdmissionDocumentId { get; set; }

    public string FileName { get; set; }

    public string PreviewUrl { get; set; }
}
