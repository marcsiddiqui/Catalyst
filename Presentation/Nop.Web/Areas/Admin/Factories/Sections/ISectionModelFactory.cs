using Nop.Core.Domain.GradeManagement;
using Nop.Web.Areas.Admin.Models.GradeManagement;

namespace Nop.Web.Areas.Admin.Factories;

public partial interface ISectionModelFactory
{
    Task<SectionSearchModel> PrepareSectionSearchModelAsync(SectionSearchModel searchModel);

    Task<SectionListModel> PrepareSectionListModelAsync(SectionSearchModel searchModel);

    Task<SectionModel> PrepareSectionModelAsync(SectionModel model, Section section, bool excludeProperties = false);
}