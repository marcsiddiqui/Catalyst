using Nop.Core.Domain.Students;
using Nop.Web.Areas.Admin.Models.Students;

namespace Nop.Web.Areas.Admin.Factories;

public partial interface IStudentModelFactory
{
    Task<StudentSearchModel> PrepareStudentSearchModelAsync(StudentSearchModel searchModel);

    Task<StudentListModel> PrepareStudentListModelAsync(StudentSearchModel searchModel);

    Task<StudentModel> PrepareStudentModelAsync(StudentModel model, Student student, bool excludeProperties = false);
}