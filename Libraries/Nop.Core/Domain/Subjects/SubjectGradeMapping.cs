using Nop.Core;

namespace Nop.Core.Domain.Subjects;

public partial class SubjectGradeMapping : BaseEntity
{
    public int GradeId { get; set; }

    public int SubjectId { get; set; }

    public decimal LabFee { get; set; }

    public int? SectionId { get; set; }


}