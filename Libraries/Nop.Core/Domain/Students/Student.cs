using Nop.Core;
using Nop.Core.Domain.Common;

namespace Nop.Core.Domain.Students;

public partial class Student : BaseEntity, ISoftDeletedEntity
{
    public int CustomerId { get; set; }

    public int PictureId { get; set; }

    public int GrNumber { get; set; }

    public string FirstName { get; set; }

    public string MiddleName { get; set; }

    public string LastName { get; set; }

    public int PreviousSchoolId { get; set; }

    public string IdentificationMark { get; set; }

    public int BirthCity { get; set; }

    public string Allergies { get; set; }

    public string MedicalNotes { get; set; }

    public int GuardianTypeId { get; set; }

    public bool IsActive { get; set; }

    public bool Deleted { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedOnUtc { get; set; }

    public int UpdatedBy { get; set; }

    public DateTime? UpdatedOnUtc { get; set; }


}