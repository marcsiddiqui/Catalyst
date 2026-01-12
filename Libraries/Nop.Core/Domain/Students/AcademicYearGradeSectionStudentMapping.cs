using Nop.Core;

namespace Nop.Core.Domain.Students;

public partial class AcademicYearGradeSectionStudentMapping : BaseEntity
{
    public int AcademicYearGradeSectionMappingId { get; set; }

    public int CustomerId { get; set; }

    public int StatusId { get; set; }

    public decimal TotalMarks { get; set; }

    public decimal ObtainedMarks { get; set; }

    public string Grade { get; set; }

    public decimal Discount { get; set; }


}