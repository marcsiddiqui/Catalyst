using Nop.Core.Domain.Subjects;
using Nop.Web.Areas.Admin.Models.Subjects;

namespace Nop.Web.Areas.Admin.Factories;

public partial interface ISubjectModelFactory
{
    Task<SubjectSearchModel> PrepareSubjectSearchModelAsync(SubjectSearchModel searchModel);

    Task<SubjectListModel> PrepareSubjectListModelAsync(SubjectSearchModel searchModel);

    Task<SubjectModel> PrepareSubjectModelAsync(SubjectModel model, Subject subject, bool excludeProperties = false);
}