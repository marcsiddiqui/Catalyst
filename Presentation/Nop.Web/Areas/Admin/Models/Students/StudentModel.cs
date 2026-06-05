using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Students;

public partial record StudentModel : BaseNopEntityModel
{
    #region Ctor

    public StudentModel()
    {
        AvailablePreviousSchools = new List<SelectListItem>();
        AvailableBirthCities = new List<SelectListItem>();
        AvailableMotherTongues = new List<SelectListItem>();
        AvailableNationalities = new List<SelectListItem>();
        AvailableReligions = new List<SelectListItem>();
        AvailableBloodGroups = new List<SelectListItem>();
        AvailableCastes = new List<SelectListItem>();
        AvailableGuardianTypes = new List<SelectListItem>();
    }

    #endregion

    #region Properties

    [NopResourceDisplayName("Admin.Configuration.Students.Fields.CustomerId")]
    public int CustomerId { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Students.Fields.PictureId")]
    public int PictureId { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Students.Fields.GrNumber")]
    public int GrNumber { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Students.Fields.Username")]
    public string Username { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Students.Fields.Email")]
    public string Email { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Students.Fields.FirstName")]
    public string FirstName { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Students.Fields.MiddleName")]
    public string MiddleName { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Students.Fields.LastName")]
    public string LastName { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Students.Fields.CNIC")]
    public string CNIC { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Students.Fields.DateOfBirth")]
    public DateTime? DateOfBirth { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Students.Fields.PhoneNumber")]
    public string PhoneNumber { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Students.Fields.PreviousSchoolId")]
    public int PreviousSchoolId { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Students.Fields.IdentificationMark")]
    public string IdentificationMark { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Students.Fields.BirthCity")]
    public int BirthCity { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Students.Fields.Allergies")]
    public string Allergies { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Students.Fields.MedicalNotes")]
    public string MedicalNotes { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Students.Fields.MontherTongue")]
    public int MontherTongue { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Students.Fields.Nationality")]
    public int Nationality { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Students.Fields.Religion")]
    public int Religion { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Students.Fields.BloodGroup")]
    public int BloodGroup { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Students.Fields.Caste")]
    public int Caste { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Students.Fields.GuardianTypeId")]
    public int GuardianTypeId { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Students.Fields.IsActive")]
    public bool IsActive { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Students.Fields.Deleted")]
    public bool Deleted { get; set; }

    public string FullName { get; set; }

    public string PictureUrl { get; set; }

    public bool RemovePicture { get; set; }

    public string TemporaryPicturePath { get; set; }

    public string TemporaryPictureFileName { get; set; }

    public string TemporaryPictureUrl { get; set; }

    public string PreviousSchool { get; set; }

    public string BirthCityName { get; set; }

    public string MontherTongueName { get; set; }

    public string NationalityName { get; set; }

    public string ReligionName { get; set; }

    public string BloodGroupName { get; set; }

    public string CasteName { get; set; }

    public string GuardianType { get; set; }

    public IList<SelectListItem> AvailablePreviousSchools { get; set; }

    public IList<SelectListItem> AvailableBirthCities { get; set; }

    public IList<SelectListItem> AvailableMotherTongues { get; set; }

    public IList<SelectListItem> AvailableNationalities { get; set; }

    public IList<SelectListItem> AvailableReligions { get; set; }

    public IList<SelectListItem> AvailableBloodGroups { get; set; }

    public IList<SelectListItem> AvailableCastes { get; set; }

    public IList<SelectListItem> AvailableGuardianTypes { get; set; }


    #endregion
}

