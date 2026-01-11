using Nop.Core.Domain.GradeManagement;
using Nop.Web.Areas.Admin.Models.GradeManagement;

namespace Nop.Web.Areas.Admin.Factories;

public partial interface IGradeModelFactory
{
    Task<GradeSearchModel> PrepareGradeSearchModelAsync(GradeSearchModel searchModel);

    Task<GradeListModel> PrepareGradeListModelAsync(GradeSearchModel searchModel);

    Task<GradeModel> PrepareGradeModelAsync(GradeModel model, Grade grade, bool excludeProperties = false);
}