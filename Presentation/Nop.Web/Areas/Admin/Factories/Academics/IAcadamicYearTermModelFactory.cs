using Nop.Core.Domain.AcademicYears;
using Nop.Web.Areas.Admin.Models.AcademicYears;

namespace Nop.Web.Areas.Admin.Factories;

public partial interface IAcademicYearTermModelFactory
{
    Task<AcademicYearTermSearchModel> PrepareAcademicYearTermSearchModelAsync(AcademicYearTermSearchModel searchModel);

    Task<AcademicYearTermListModel> PrepareAcademicYearTermListModelAsync(AcademicYearTermSearchModel searchModel);

    Task<AcademicYearTermModel> PrepareAcademicYearTermModelAsync(AcademicYearTermModel model, AcademicYearTerm academicYearTerm, bool excludeProperties = false);
}