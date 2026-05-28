using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Admissions;

public partial record AdmissionModel : BaseNopEntityModel
{
    #region Ctor

    public AdmissionModel()
    {
        AvailableAdmissionStatuses = new List<SelectListItem>();
        AvailablePreviousSchools = new List<SelectListItem>();
        AvailableBirthCities = new List<SelectListItem>();
        AvailableMotherTongues = new List<SelectListItem>();
        AvailableNationalities = new List<SelectListItem>();
        AvailableReligions = new List<SelectListItem>();
        AvailableBloodGroups = new List<SelectListItem>();
        AvailableCastes = new List<SelectListItem>();
        AvailableGuardianTypes = new List<SelectListItem>();
        AvailableQualifications = new List<SelectListItem>();
        AvailableProfessions = new List<SelectListItem>();
        AvailableGrades = new List<SelectListItem>();
        RequiredDocuments = new List<AdmissionRequiredDocumentModel>();
    }

    #endregion

    #region Properties

    [NopResourceDisplayName("Admin.Configuration.Admissions.Fields.FormNo")]
    public string FormNo { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Admissions.Fields.StatusId")]
    public int StatusId { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Admissions.Fields.GradeId")]
    public int GradeId { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Admissions.Fields.FirstName")]
    public string FirstName { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Admissions.Fields.MiddleName")]
    public string MiddleName { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Admissions.Fields.LastName")]
    public string LastName { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Admissions.Fields.CNIC")]
    public string CNIC { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Admissions.Fields.PreviousSchoolId")]
    public int PreviousSchoolId { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Admissions.Fields.IdentificationMark")]
    public string IdentificationMark { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Admissions.Fields.DateOfBirth")]
    public DateTime DateOfBirth { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Admissions.Fields.BirthCity")]
    public int BirthCity { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Admissions.Fields.Allergies")]
    public string Allergies { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Admissions.Fields.MedicalNotes")]
    public string MedicalNotes { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Admissions.Fields.MontherTongue")]
    public int MontherTongue { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Admissions.Fields.Nationality")]
    public int Nationality { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Admissions.Fields.Religion")]
    public int Religion { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Admissions.Fields.BloodGroup")]
    public int BloodGroup { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Admissions.Fields.Caste")]
    public int Caste { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Admissions.Fields.GuardianTypeId")]
    public int GuardianTypeId { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Admissions.Fields.FatherFullName")]
    public string FatherFullName { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Admissions.Fields.FatherCNIC")]
    public string FatherCNIC { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Admissions.Fields.FatherDateOfBirth")]
    public DateTime FatherDateOfBirth { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Admissions.Fields.FatherIsDeceased")]
    public bool FatherIsDeceased { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Admissions.Fields.FatherQaulification")]
    public int FatherQaulification { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Admissions.Fields.FatherPhoneNo")]
    public string FatherPhoneNo { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Admissions.Fields.FatherProfession")]
    public int FatherProfession { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Admissions.Fields.FatherMonthlyIncome")]
    public decimal FatherMonthlyIncome { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Admissions.Fields.Father_MontherTongue")]
    public int Father_MontherTongue { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Admissions.Fields.FatherNationality")]
    public int FatherNationality { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Admissions.Fields.FatherReligion")]
    public int FatherReligion { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Admissions.Fields.FatherBloodGroup")]
    public int FatherBloodGroup { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Admissions.Fields.FatherCaste")]
    public int FatherCaste { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Admissions.Fields.MotherFullName")]
    public string MotherFullName { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Admissions.Fields.MotherCNIC")]
    public string MotherCNIC { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Admissions.Fields.MotherDateOfBirth")]
    public DateTime MotherDateOfBirth { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Admissions.Fields.MotherIsDeceased")]
    public bool MotherIsDeceased { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Admissions.Fields.MotherQaulification")]
    public int MotherQaulification { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Admissions.Fields.MotherPhoneNo")]
    public string MotherPhoneNo { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Admissions.Fields.MotherProfession")]
    public int MotherProfession { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Admissions.Fields.MotherMonthlyIncome")]
    public decimal MotherMonthlyIncome { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Admissions.Fields.Mother_MontherTongue")]
    public int Mother_MontherTongue { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Admissions.Fields.MotherNationality")]
    public int MotherNationality { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Admissions.Fields.MotherReligion")]
    public int MotherReligion { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Admissions.Fields.MotherBloodGroup")]
    public int MotherBloodGroup { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Admissions.Fields.MotherCaste")]
    public int MotherCaste { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Admissions.Fields.GuardianFullName")]
    public string GuardianFullName { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Admissions.Fields.GuardianCNIC")]
    public string GuardianCNIC { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Admissions.Fields.GuardianDateOfBirth")]
    public DateTime GuardianDateOfBirth { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Admissions.Fields.GuardianIsDeceased")]
    public bool GuardianIsDeceased { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Admissions.Fields.GuardianQaulification")]
    public int GuardianQaulification { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Admissions.Fields.GuardianPhoneNo")]
    public string GuardianPhoneNo { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Admissions.Fields.GuardianProfession")]
    public int GuardianProfession { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Admissions.Fields.GuardianMonthlyIncome")]
    public decimal GuardianMonthlyIncome { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Admissions.Fields.Guardian_MontherTongue")]
    public int Guardian_MontherTongue { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Admissions.Fields.GuardianNationality")]
    public int GuardianNationality { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Admissions.Fields.GuardianReligion")]
    public int GuardianReligion { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Admissions.Fields.GuardianBloodGroup")]
    public int GuardianBloodGroup { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Admissions.Fields.GuardianCaste")]
    public int GuardianCaste { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Admissions.Fields.Createdby")]
    public int Createdby { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Admissions.Fields.CreatedOnUtc")]
    public DateTime CreatedOnUtc { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Admissions.Fields.UpdatedBy")]
    public int UpdatedBy { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Admissions.Fields.UpdatedOnUtc")]
    public DateTime? UpdatedOnUtc { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Admissions.Fields.Deleted")]
    public bool Deleted { get; set; }

    public string Status { get; set; }

    public string PreviousSchool { get; set; }

    public string BirthCityName { get; set; }

    public string MontherTongueName { get; set; }

    public string NationalityName { get; set; }

    public string ReligionName { get; set; }

    public string BloodGroupName { get; set; }

    public string CasteName { get; set; }

    public string GuardianType { get; set; }

    public IList<SelectListItem> AvailableAdmissionStatuses { get; set; }

    public IList<SelectListItem> AvailablePreviousSchools { get; set; }

    public IList<SelectListItem> AvailableBirthCities { get; set; }

    public IList<SelectListItem> AvailableMotherTongues { get; set; }

    public IList<SelectListItem> AvailableNationalities { get; set; }

    public IList<SelectListItem> AvailableReligions { get; set; }

    public IList<SelectListItem> AvailableBloodGroups { get; set; }

    public IList<SelectListItem> AvailableCastes { get; set; }

    public IList<SelectListItem> AvailableGuardianTypes { get; set; }

    public IList<SelectListItem> AvailableQualifications { get; set; }

    public IList<SelectListItem> AvailableProfessions { get; set; }

    public IList<SelectListItem> AvailableGrades { get; set; }

    public IList<AdmissionRequiredDocumentModel> RequiredDocuments { get; set; }

    #endregion
}

