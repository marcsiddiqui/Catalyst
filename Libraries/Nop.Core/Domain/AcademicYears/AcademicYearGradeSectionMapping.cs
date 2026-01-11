using Nop.Core;

namespace Nop.Core.Domain.AcademicYears;

public partial class AcademicYearGradeSectionMapping : BaseEntity
{
    public int AcademicYearId { get; set; }

    public int GradeId { get; set; }

    public int SectionId { get; set; }

    public int ExamTermCount { get; set; }


}