using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.GradeManagement;

public partial record GradeCopySetupModel : BaseNopModel
{
    public GradeCopySetupModel()
    {
        AvailableGrades = new List<SelectListItem>();
    }

    [NopResourceDisplayName("Admin.Grade.CopySetup.Fields.FromGradeId")]
    public int FromGradeId { get; set; }

    public int ToGradeId { get; set; }

    [NopResourceDisplayName("Admin.Grade.CopySetup.Fields.CopyGradeSubjectMapping")]
    public bool CopyGradeSubjectMapping { get; set; }

    [NopResourceDisplayName("Admin.Grade.CopySetup.Fields.CopyAdmissionDocumentRequirements")]
    public bool CopyAdmissionDocumentRequirements { get; set; }

    public bool IsEditMode { get; set; }

    public IList<SelectListItem> AvailableGrades { get; set; }
}
