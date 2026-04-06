using Nop.Core;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Subjects;

namespace Nop.Services.Subjects;

public partial interface ISubjectGradeMappingService
{
    Task<IPagedList<SubjectGradeMapping>> GetAllSubjectGradeMappingsAsync(
        int id = 0, IEnumerable<int> ids = null,
        int gradeId = 0, IEnumerable<int> gradeIds = null,
        int subjectId = 0, IEnumerable<int> subjectIds = null,
        int sectionId = 0, IEnumerable<int> sectionIds = null,

        int pageIndex = 0, int pageSize = int.MaxValue);

    Task<SubjectGradeMapping> GetSubjectGradeMappingByIdAsync(int id);

    Task<IList<SubjectGradeMapping>> GetSubjectGradeMappingsByIdsAsync(IEnumerable<int> ids);

    Task InsertSubjectGradeMappingAsync(SubjectGradeMapping subjectGradeMapping);
    
    Task InsertSubjectGradeMappingAsync(IEnumerable<SubjectGradeMapping> subjectGradeMappings);

    Task UpdateSubjectGradeMappingAsync(SubjectGradeMapping subjectGradeMapping);

    Task UpdateSubjectGradeMappingAsync(IEnumerable<SubjectGradeMapping> subjectGradeMappings);

    Task DeleteSubjectGradeMappingAsync(SubjectGradeMapping subjectGradeMapping);

    Task DeleteSubjectGradeMappingAsync(IEnumerable<SubjectGradeMapping> subjectGradeMappings);
}