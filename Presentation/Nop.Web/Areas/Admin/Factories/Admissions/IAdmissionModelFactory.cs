using Nop.Core.Domain.Admissions;
using Nop.Web.Areas.Admin.Models.Admissions;

namespace Nop.Web.Areas.Admin.Factories;

public partial interface IAdmissionModelFactory
{
    Task<AdmissionSearchModel> PrepareAdmissionSearchModelAsync(AdmissionSearchModel searchModel);

    Task<AdmissionListModel> PrepareAdmissionListModelAsync(AdmissionSearchModel searchModel);

    Task<AdmissionModel> PrepareAdmissionModelAsync(AdmissionModel model, Admission admission, bool excludeProperties = false);

    Task<AdmissionGradeDocumentRequirementListModel> PrepareAdmissionGradeDocumentRequirementListModelAsync(AdmissionGradeDocumentRequirementSearchModel searchModel);

    Task<AdmissionGradeDocumentRequirementModel> PrepareAdmissionGradeDocumentRequirementModelAsync(AdmissionGradeDocumentRequirementModel model,
        AdmissionGradeDocumentRequirement admissionGradeDocumentRequirement,
        bool excludeProperties = false);
}
