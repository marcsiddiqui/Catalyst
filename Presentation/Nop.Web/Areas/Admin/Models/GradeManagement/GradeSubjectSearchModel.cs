using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.GradeManagement;

public partial record GradeSubjectSearchModel : BaseSearchModel
{
    #region Properties

    public int GradeId { get; set; }

    #endregion
}