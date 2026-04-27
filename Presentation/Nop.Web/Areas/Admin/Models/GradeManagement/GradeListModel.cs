using Nop.Web.Framework.Models;

namespace Nop.Web.Areas.Admin.Models.GradeManagement;

public partial record GradeListModel : BasePagedListModel<GradeModel>;

public partial record GradeSubjectListModel : BasePagedListModel<GradeSubjectMappingModel>;