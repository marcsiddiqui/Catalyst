using Nop.Core;
using Nop.Core.Domain.Admissions;
using Nop.Data;

namespace Nop.Services.Admissions;

public partial class AdmissionService : IAdmissionService
{
    #region Fields

    protected readonly IRepository<Admission> _admissionRepository;
    protected readonly IRepository<AdmissionGradeDocumentRequirement> _admissionGradeDocumentRequirementRepository;

    #endregion

    #region Ctor

    public AdmissionService(
        IRepository<Admission> admissionRepository,
        IRepository<AdmissionGradeDocumentRequirement> admissionGradeDocumentRequirementRepository
        )
    {
        _admissionRepository = admissionRepository;
        _admissionGradeDocumentRequirementRepository = admissionGradeDocumentRequirementRepository;
    }

    #endregion

    #region Methods

    public virtual async Task<IPagedList<Admission>> GetAllAdmissionsAsync(
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

        int pageIndex = 0, int pageSize = int.MaxValue)
    {
        var productReviews = await _admissionRepository.GetAllPagedAsync(async query =>
        {
            if (id > 0)
                query = query.Where(x => x.Id == id);

            if (ids != null && ids.Any())
                query = query.Where(x => ids.Contains(x.Id));

            if (!string.IsNullOrWhiteSpace(formNo))
                query = query.Where(x => formNo == x.FormNo);

            if (formNos != null && formNos.Any())
                query = query.Where(x => formNos.Contains(x.FormNo));

            if (statusId > 0)
                query = query.Where(x => statusId == x.StatusId);

            if (statusIds != null && statusIds.Any())
                query = query.Where(x => statusIds.Contains(x.StatusId));

            if (!string.IsNullOrWhiteSpace(firstName))
                query = query.Where(x => firstName == x.FirstName);

            if (firstNames != null && firstNames.Any())
                query = query.Where(x => firstNames.Contains(x.FirstName));

            if (!string.IsNullOrWhiteSpace(middleName))
                query = query.Where(x => middleName == x.MiddleName);

            if (middleNames != null && middleNames.Any())
                query = query.Where(x => middleNames.Contains(x.MiddleName));

            if (!string.IsNullOrWhiteSpace(lastName))
                query = query.Where(x => lastName == x.LastName);

            if (lastNames != null && lastNames.Any())
                query = query.Where(x => lastNames.Contains(x.LastName));

            if (!string.IsNullOrWhiteSpace(cNIC))
                query = query.Where(x => cNIC == x.CNIC);

            if (cNICs != null && cNICs.Any())
                query = query.Where(x => cNICs.Contains(x.CNIC));

            if (previousSchoolId > 0)
                query = query.Where(x => previousSchoolId == x.PreviousSchoolId);

            if (previousSchoolIds != null && previousSchoolIds.Any())
                query = query.Where(x => previousSchoolIds.Contains(x.PreviousSchoolId));

            if (!string.IsNullOrWhiteSpace(identificationMark))
                query = query.Where(x => identificationMark == x.IdentificationMark);

            if (identificationMarks != null && identificationMarks.Any())
                query = query.Where(x => identificationMarks.Contains(x.IdentificationMark));


            if (!string.IsNullOrWhiteSpace(allergies))
                query = query.Where(x => allergies == x.Allergies);

            if (allergieses != null && allergieses.Any())
                query = query.Where(x => allergieses.Contains(x.Allergies));

            if (!string.IsNullOrWhiteSpace(medicalNotes))
                query = query.Where(x => medicalNotes == x.MedicalNotes);

            if (medicalNoteses != null && medicalNoteses.Any())
                query = query.Where(x => medicalNoteses.Contains(x.MedicalNotes));






            if (guardianTypeId > 0)
                query = query.Where(x => guardianTypeId == x.GuardianTypeId);

            if (guardianTypeIds != null && guardianTypeIds.Any())
                query = query.Where(x => guardianTypeIds.Contains(x.GuardianTypeId));

            if (!string.IsNullOrWhiteSpace(fatherFullName))
                query = query.Where(x => fatherFullName == x.FatherFullName);

            if (fatherFullNames != null && fatherFullNames.Any())
                query = query.Where(x => fatherFullNames.Contains(x.FatherFullName));

            if (!string.IsNullOrWhiteSpace(fatherCNIC))
                query = query.Where(x => fatherCNIC == x.FatherCNIC);

            if (fatherCNICs != null && fatherCNICs.Any())
                query = query.Where(x => fatherCNICs.Contains(x.FatherCNIC));

            query = query.WhereBoolean(x => x.FatherIsDeceased, fatherIsDeceased);


            if (!string.IsNullOrWhiteSpace(fatherPhoneNo))
                query = query.Where(x => fatherPhoneNo == x.FatherPhoneNo);

            if (fatherPhoneNos != null && fatherPhoneNos.Any())
                query = query.Where(x => fatherPhoneNos.Contains(x.FatherPhoneNo));







            if (!string.IsNullOrWhiteSpace(motherFullName))
                query = query.Where(x => motherFullName == x.MotherFullName);

            if (motherFullNames != null && motherFullNames.Any())
                query = query.Where(x => motherFullNames.Contains(x.MotherFullName));

            if (!string.IsNullOrWhiteSpace(motherCNIC))
                query = query.Where(x => motherCNIC == x.MotherCNIC);

            if (motherCNICs != null && motherCNICs.Any())
                query = query.Where(x => motherCNICs.Contains(x.MotherCNIC));

            query = query.WhereBoolean(x => x.MotherIsDeceased, motherIsDeceased);


            if (!string.IsNullOrWhiteSpace(motherPhoneNo))
                query = query.Where(x => motherPhoneNo == x.MotherPhoneNo);

            if (motherPhoneNos != null && motherPhoneNos.Any())
                query = query.Where(x => motherPhoneNos.Contains(x.MotherPhoneNo));







            if (!string.IsNullOrWhiteSpace(guardianFullName))
                query = query.Where(x => guardianFullName == x.GuardianFullName);

            if (guardianFullNames != null && guardianFullNames.Any())
                query = query.Where(x => guardianFullNames.Contains(x.GuardianFullName));

            if (!string.IsNullOrWhiteSpace(guardianCNIC))
                query = query.Where(x => guardianCNIC == x.GuardianCNIC);

            if (guardianCNICs != null && guardianCNICs.Any())
                query = query.Where(x => guardianCNICs.Contains(x.GuardianCNIC));

            query = query.WhereBoolean(x => x.GuardianIsDeceased, guardianIsDeceased);


            if (!string.IsNullOrWhiteSpace(guardianPhoneNo))
                query = query.Where(x => guardianPhoneNo == x.GuardianPhoneNo);

            if (guardianPhoneNos != null && guardianPhoneNos.Any())
                query = query.Where(x => guardianPhoneNos.Contains(x.GuardianPhoneNo));









            query = query.WhereBoolean(x => x.Deleted, deleted);



            return query;

        }, pageIndex, pageSize);

        return productReviews;
    }

    public virtual async Task<Admission> GetAdmissionByIdAsync(int id)
    {
        if (id <= 0)
            return null;

        return await _admissionRepository.GetByIdAsync(id);
    }

    public virtual async Task<IList<Admission>> GetAdmissionsByIdsAsync(IEnumerable<int> ids)
    {
        if (ids == null || !ids.Any())
            return null;

        return await _admissionRepository.GetByIdsAsync(ids.ToList());
    }

    public virtual async Task InsertAdmissionAsync(Admission admission)
    {
        if (admission == null)
            return;

        await _admissionRepository.InsertAsync(admission);
    }
    
    public virtual async Task InsertAdmissionAsync(IEnumerable<Admission> admissions)
    {
        if (admissions == null || !admissions.Any())
            return;

        await _admissionRepository.InsertAsync(admissions.ToList());
    }

    public virtual async Task UpdateAdmissionAsync(Admission admission)
    {
        if (admission == null)
            return;

        await _admissionRepository.UpdateAsync(admission);
    }

    public virtual async Task UpdateAdmissionAsync(IEnumerable<Admission> admissions)
    {
        if (admissions == null || !admissions.Any())
            return;

        await _admissionRepository.UpdateAsync(admissions.ToList());
    }

    public virtual async Task DeleteAdmissionAsync(Admission admission)
    {
        if (admission == null)
            return;

        await _admissionRepository.DeleteAsync(admission);
    }

    public virtual async Task DeleteAdmissionAsync(IEnumerable<Admission> admissions)
    {
        if (admissions == null || !admissions.Any())
            return;

        await _admissionRepository.DeleteAsync(admissions.ToList());
    }

    #endregion

    #region AdmissionGradeDocumentRequirement

    public virtual async Task<IPagedList<AdmissionGradeDocumentRequirement>> GetAllAdmissionGradeDocumentRequirementsAsync(
        int id = 0, IEnumerable<int> ids = null,
        int gradeId = 0, IEnumerable<int> gradeIds = null,
        int admissionDocumentTypeId = 0, IEnumerable<int> admissionDocumentTypeIds = null,
        BooleanFilter isRequired = BooleanFilter.Both,
        BooleanFilter deleted = BooleanFilter.False,



        int pageIndex = 0, int pageSize = int.MaxValue)
    {
        var productReviews = await _admissionGradeDocumentRequirementRepository.GetAllPagedAsync(async query =>
        {
            if (id > 0)
                query = query.Where(x => x.Id == id);

            if (ids != null && ids.Any())
                query = query.Where(x => ids.Contains(x.Id));

            if (gradeId > 0)
                query = query.Where(x => gradeId == x.GradeId);

            if (gradeIds != null && gradeIds.Any())
                query = query.Where(x => gradeIds.Contains(x.GradeId));

            if (admissionDocumentTypeId > 0)
                query = query.Where(x => admissionDocumentTypeId == x.AdmissionDocumentTypeId);

            if (admissionDocumentTypeIds != null && admissionDocumentTypeIds.Any())
                query = query.Where(x => admissionDocumentTypeIds.Contains(x.AdmissionDocumentTypeId));

            query = query.WhereBoolean(x => x.IsRequired, isRequired);

            query = query.WhereBoolean(x => x.Deleted, deleted);





            return query;

        }, pageIndex, pageSize);

        return productReviews;
    }

    public virtual async Task<AdmissionGradeDocumentRequirement> GetAdmissionGradeDocumentRequirementByIdAsync(int id)
    {
        if (id <= 0)
            return null;

        return await _admissionGradeDocumentRequirementRepository.GetByIdAsync(id);
    }

    public virtual async Task<IList<AdmissionGradeDocumentRequirement>> GetAdmissionGradeDocumentRequirementsByIdsAsync(IEnumerable<int> ids)
    {
        if (ids == null || !ids.Any())
            return null;

        return await _admissionGradeDocumentRequirementRepository.GetByIdsAsync(ids.ToList());
    }

    public virtual async Task InsertAdmissionGradeDocumentRequirementAsync(AdmissionGradeDocumentRequirement admissionGradeDocumentRequirement)
    {
        if (admissionGradeDocumentRequirement == null)
            return;

        await _admissionGradeDocumentRequirementRepository.InsertAsync(admissionGradeDocumentRequirement);
    }

    public virtual async Task InsertAdmissionGradeDocumentRequirementAsync(IEnumerable<AdmissionGradeDocumentRequirement> admissionGradeDocumentRequirements)
    {
        if (admissionGradeDocumentRequirements == null || !admissionGradeDocumentRequirements.Any())
            return;

        await _admissionGradeDocumentRequirementRepository.InsertAsync(admissionGradeDocumentRequirements.ToList());
    }

    public virtual async Task UpdateAdmissionGradeDocumentRequirementAsync(AdmissionGradeDocumentRequirement admissionGradeDocumentRequirement)
    {
        if (admissionGradeDocumentRequirement == null)
            return;

        await _admissionGradeDocumentRequirementRepository.UpdateAsync(admissionGradeDocumentRequirement);
    }

    public virtual async Task UpdateAdmissionGradeDocumentRequirementAsync(IEnumerable<AdmissionGradeDocumentRequirement> admissionGradeDocumentRequirements)
    {
        if (admissionGradeDocumentRequirements == null || !admissionGradeDocumentRequirements.Any())
            return;

        await _admissionGradeDocumentRequirementRepository.UpdateAsync(admissionGradeDocumentRequirements.ToList());
    }

    public virtual async Task DeleteAdmissionGradeDocumentRequirementAsync(AdmissionGradeDocumentRequirement admissionGradeDocumentRequirement)
    {
        if (admissionGradeDocumentRequirement == null)
            return;

        await _admissionGradeDocumentRequirementRepository.DeleteAsync(admissionGradeDocumentRequirement);
    }

    public virtual async Task DeleteAdmissionGradeDocumentRequirementAsync(IEnumerable<AdmissionGradeDocumentRequirement> admissionGradeDocumentRequirements)
    {
        if (admissionGradeDocumentRequirements == null || !admissionGradeDocumentRequirements.Any())
            return;

        await _admissionGradeDocumentRequirementRepository.DeleteAsync(admissionGradeDocumentRequirements.ToList());
    }

    #endregion
}