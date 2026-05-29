using Nop.Core;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.LogInfo;

namespace Nop.Core.Domain.Admissions;

public partial class Admission : LogInfoSupportedBaseEntity, ISoftDeletedEntity
{
    public string FormNo { get; set; }

    public int SRN { get; set; }

    public int AcademicYearId { get; set; }

    public int StatusId { get; set; }

    public int GradeId { get; set; }

    public string FirstName { get; set; }

    public string MiddleName { get; set; }

    public string LastName { get; set; }

    public string CNIC { get; set; }

    public int PreviousSchoolId { get; set; }

    public string IdentificationMark { get; set; }

    public DateTime DateOfBirth { get; set; }

    public int BirthCity { get; set; }

    public string Address { get; set; }

    public int SiblingsCount { get; set; }

    public int NoInSiblings { get; set; }

    public string Allergies { get; set; }

    public string MedicalNotes { get; set; }

    public int MontherTongue { get; set; }

    public int Nationality { get; set; }

    public int Religion { get; set; }

    public int BloodGroup { get; set; }

    public int Caste { get; set; }

    public int GuardianTypeId { get; set; }

    public string FatherFullName { get; set; }

    public string FatherCNIC { get; set; }

    public DateTime FatherDateOfBirth { get; set; }

    public bool FatherIsDeceased { get; set; }

    public int FatherQaulification { get; set; }

    public string FatherPhoneNo { get; set; }

    public int FatherProfession { get; set; }

    public string FatherOfficeAddress { get; set; }

    public string FatherOfficePhoneNumber { get; set; }

    public decimal FatherMonthlyIncome { get; set; }

    public int Father_MontherTongue { get; set; }

    public int FatherNationality { get; set; }

    public int FatherReligion { get; set; }

    public int FatherBloodGroup { get; set; }

    public int FatherCaste { get; set; }

    public string MotherFullName { get; set; }

    public string MotherCNIC { get; set; }

    public DateTime MotherDateOfBirth { get; set; }

    public bool MotherIsDeceased { get; set; }

    public int MotherQaulification { get; set; }

    public string MotherPhoneNo { get; set; }

    public int MotherProfession { get; set; }

    public decimal MotherMonthlyIncome { get; set; }

    public string MotherOfficeAddress { get; set; }

    public string MotherOfficePhoneNumber { get; set; }

    public int Mother_MontherTongue { get; set; }

    public int MotherNationality { get; set; }

    public int MotherReligion { get; set; }

    public int MotherBloodGroup { get; set; }

    public int MotherCaste { get; set; }

    public string GuardianFullName { get; set; }

    public string GuardianCNIC { get; set; }

    public DateTime GuardianDateOfBirth { get; set; }

    public bool GuardianIsDeceased { get; set; }

    public int GuardianQaulification { get; set; }

    public string GuardianPhoneNo { get; set; }

    public int GuardianProfession { get; set; }

    public decimal GuardianMonthlyIncome { get; set; }

    public int Guardian_MontherTongue { get; set; }

    public int GuardianNationality { get; set; }

    public int GuardianReligion { get; set; }

    public int GuardianBloodGroup { get; set; }

    public int GuardianCaste { get; set; }

    public bool Deleted { get; set; }
}