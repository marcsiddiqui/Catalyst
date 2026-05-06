using Nop.Web.Areas.Admin.Models.GradeManagement;
using Nop.Web.Framework.Models;

namespace Nop.Web.Areas.Admin.Models.AcademicYears;

public partial record AcademicYearListModel : BasePagedListModel<AcademicYearModel>;

public partial record AcademicYearGradeSectionMappingListModel : BasePagedListModel<AcademicYearGradeSectionMappingModel>;