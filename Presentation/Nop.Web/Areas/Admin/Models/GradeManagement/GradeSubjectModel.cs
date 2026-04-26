using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.GradeManagement;

public partial record GradeSubjectModel : BaseNopEntityModel
{
    #region Properties

    public int GradeId { get; set; }

    public int SubjectId { get; set; }

    public string SubjectName { get; set; }

    public int? SectionId { get; set; }

    public string SectionName { get; set; }

    public decimal LabFee { get; set; }

    public bool Deleted { get; set; }

    #endregion
}