using Nop.Core;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.LogInfo;

namespace Nop.Core.Domain.AcademicYears;

public partial class AcademicYearGradeSectionMapping : LogInfoSupportedBaseEntity, ISoftDeletedEntity
{
    public int AcademicYearId { get; set; }

    public int GradeId { get; set; }

    public int SectionId { get; set; }

    public int ExamTermCount { get; set; }

    public bool Deleted { get; set; }
}