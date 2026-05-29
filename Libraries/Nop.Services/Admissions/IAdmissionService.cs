using Nop.Core;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Admissions;

namespace Nop.Services.Admissions;

public partial interface IAdmissionService
{
    Task<IPagedList<Admission>> GetAllAdmissionsAsync(
        int id = 0, IEnumerable<int> ids = null,
        string formNo = null, IEnumerable<string> formNos = null,
        int statusId = 0, IEnumerable<int> statusIds = null,
        string firstName = null, IEnumerable<string> firstNames = null,
        string middleName = null, IEnumerable<string> middleNames = null,
        string lastName = null, IEnumerable<string> lastNames = null,
        string cNIC = null, IEnumerable<string> cNICs = null,
        int previousSchoolId = 0, IEnumerable<int> previousSchoolIds = null,
        string identificationMark = null, IEnumerable<string> identificationMarks = null,

        string allergies = null, IEnumerable<string> allergieses = null,
        string medicalNotes = null, IEnumerable<string> medicalNoteses = null,





        int guardianTypeId = 0, IEnumerable<int> guardianTypeIds = null,
        string fatherFullName = null, IEnumerable<string> fatherFullNames = null,
        string fatherCNIC = null, IEnumerable<string> fatherCNICs = null,
        BooleanFilter fatherIsDeceased = BooleanFilter.Both,

        string fatherPhoneNo = null, IEnumerable<string> fatherPhoneNos = null,






        string motherFullName = null, IEnumerable<string> motherFullNames = null,
        string motherCNIC = null, IEnumerable<string> motherCNICs = null,
        BooleanFilter motherIsDeceased = BooleanFilter.Both,

        string motherPhoneNo = null, IEnumerable<string> motherPhoneNos = null,






        string guardianFullName = null, IEnumerable<string> guardianFullNames = null,
        string guardianCNIC = null, IEnumerable<string> guardianCNICs = null,
        BooleanFilter guardianIsDeceased = BooleanFilter.Both,

        string guardianPhoneNo = null, IEnumerable<string> guardianPhoneNos = null,








        BooleanFilter deleted = BooleanFilter.False,

        int pageIndex = 0, int pageSize = int.MaxValue);

    Task<Admission> GetAdmissionByIdAsync(int id);

    Task<IList<Admission>> GetAdmissionsByIdsAsync(IEnumerable<int> ids);

    Task<int> GenerateAdmissionSrnAsync();

    Task InsertAdmissionAsync(Admission admission);
    
    Task InsertAdmissionAsync(IEnumerable<Admission> admissions);

    Task UpdateAdmissionAsync(Admission admission);

    Task UpdateAdmissionAsync(IEnumerable<Admission> admissions);

    Task DeleteAdmissionAsync(Admission admission);

    Task DeleteAdmissionAsync(IEnumerable<Admission> admissions);

    #region AdmissionGradeDocumentRequirement

    Task<IPagedList<AdmissionGradeDocumentRequirement>> GetAllAdmissionGradeDocumentRequirementsAsync(
        int id = 0, IEnumerable<int> ids = null,
        int gradeId = 0, IEnumerable<int> gradeIds = null,
        int admissionDocumentTypeId = 0, IEnumerable<int> admissionDocumentTypeIds = null,
        BooleanFilter isRequired = BooleanFilter.Both,
        BooleanFilter deleted = BooleanFilter.False,



        int pageIndex = 0, int pageSize = int.MaxValue);

    Task<AdmissionGradeDocumentRequirement> GetAdmissionGradeDocumentRequirementByIdAsync(int id);

    Task<IList<AdmissionGradeDocumentRequirement>> GetAdmissionGradeDocumentRequirementsByIdsAsync(IEnumerable<int> ids);

    Task InsertAdmissionGradeDocumentRequirementAsync(AdmissionGradeDocumentRequirement admissionGradeDocumentRequirement);

    Task InsertAdmissionGradeDocumentRequirementAsync(IEnumerable<AdmissionGradeDocumentRequirement> admissionGradeDocumentRequirements);

    Task UpdateAdmissionGradeDocumentRequirementAsync(AdmissionGradeDocumentRequirement admissionGradeDocumentRequirement);

    Task UpdateAdmissionGradeDocumentRequirementAsync(IEnumerable<AdmissionGradeDocumentRequirement> admissionGradeDocumentRequirements);

    Task DeleteAdmissionGradeDocumentRequirementAsync(AdmissionGradeDocumentRequirement admissionGradeDocumentRequirement);

    Task DeleteAdmissionGradeDocumentRequirementAsync(IEnumerable<AdmissionGradeDocumentRequirement> admissionGradeDocumentRequirements);

    #endregion
}
