using Nop.Core.Domain.AcademicYears;
using Nop.Web.Areas.Admin.Models.AcademicYears;

namespace Nop.Web.Areas.Admin.Factories;

public partial interface IAcademicYearModelFactory
{
    Task<AcademicYearSearchModel> PrepareAcademicYearSearchModelAsync(AcademicYearSearchModel searchModel);

    Task<AcademicYearListModel> PrepareAcademicYearListModelAsync(AcademicYearSearchModel searchModel);

    Task<AcademicYearModel> PrepareAcademicYearModelAsync(AcademicYearModel model, AcademicYear academicYear, bool excludeProperties = false);
}