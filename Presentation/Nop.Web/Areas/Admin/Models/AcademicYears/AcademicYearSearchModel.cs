using Nop.Web.Framework.Models;

namespace Nop.Web.Areas.Admin.Models.AcademicYears;

public partial record AcademicYearSearchModel : BaseSearchModel;

public partial record AcademicYearGradeSectionMappingSearchModel : BaseSearchModel
{
    #region Properties

    public int AcademicYearId { get; set; }

    #endregion
}