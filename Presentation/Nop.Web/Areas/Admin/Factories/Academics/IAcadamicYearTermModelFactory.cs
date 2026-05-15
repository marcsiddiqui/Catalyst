using Nop.Core.Domain.AcademicYears;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Areas.Admin.Models.AcademicYears;

namespace Nop.Web.Areas.Admin.Factories;

public partial interface IAcademicYearTermModelFactory
{
    Task<IDictionary<int, string>> PrepareAcademicYearGradeSectionMappingNamesAsync(IEnumerable<AcademicYearGradeSectionMapping> mappings);

    Task PrepareAcademicYearGradeSectionMappingsAsync(IList<SelectListItem> items, int selectedMappingId = 0);

    Task<AcademicYearTermSearchModel> PrepareAcademicYearTermSearchModelAsync(AcademicYearTermSearchModel searchModel);

    Task<AcademicYearTermListModel> PrepareAcademicYearTermListModelAsync(AcademicYearTermSearchModel searchModel);

    Task<AcademicYearTermModel> PrepareAcademicYearTermModelAsync(AcademicYearTermModel model, AcademicYearTerm academicYearTerm, bool excludeProperties = false);
}
