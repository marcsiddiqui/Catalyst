using Nop.Core.Domain.AcademicYears;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.AcademicYears;
using Nop.Web.Areas.Admin.Models.GradeManagement;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Web.Areas.Admin.Factories;

public partial interface IAcademicYearModelFactory
{
    Task<AcademicYearSearchModel> PrepareAcademicYearSearchModelAsync(AcademicYearSearchModel searchModel);

    Task<AcademicYearListModel> PrepareAcademicYearListModelAsync(AcademicYearSearchModel searchModel);

    Task<AcademicYearModel> PrepareAcademicYearModelAsync(AcademicYearModel model, AcademicYear academicYear, bool excludeProperties = false);

    #region AcademicYearGradeSectionMapping

    Task<AcademicYearGradeSectionMappingListModel> PrepareAcademicYearGradeSectionMappingListModelAsync(AcademicYearGradeSectionMappingSearchModel searchModel);

    Task<AcademicYearGradeSectionMappingModel> PrepareAcademicYearGradeSectionMappingModelAsync(AcademicYearGradeSectionMappingModel model, AcademicYearGradeSectionMapping grade, bool excludeProperties = false);

    #endregion
}