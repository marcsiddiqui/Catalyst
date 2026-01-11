using Nop.Core.Domain.AcademicYears;
using Nop.Web.Areas.Admin.Models.AcademicYears;

namespace Nop.Web.Areas.Admin.Factories;

public partial interface IAcadamicYearTermModelFactory
{
    Task<AcadamicYearTermSearchModel> PrepareAcadamicYearTermSearchModelAsync(AcadamicYearTermSearchModel searchModel);

    Task<AcadamicYearTermListModel> PrepareAcadamicYearTermListModelAsync(AcadamicYearTermSearchModel searchModel);

    Task<AcadamicYearTermModel> PrepareAcadamicYearTermModelAsync(AcadamicYearTermModel model, AcadamicYearTerm acadamicYearTerm, bool excludeProperties = false);
}