using Nop.Core;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Students;

namespace Nop.Services.Students;

public partial interface IAcademicYearGradeSectionStudentMappingService
{
    Task<IPagedList<AcademicYearGradeSectionStudentMapping>> GetAllAcademicYearGradeSectionStudentMappingsAsync(
        int id = 0, IEnumerable<int> ids = null,
        int academicYearGradeSectionMappingId = 0, IEnumerable<int> academicYearGradeSectionMappingIds = null,
        int customerId = 0, IEnumerable<int> customerIds = null,
        int statusId = 0, IEnumerable<int> statusIds = null,
        string grade = null, IEnumerable<string> grades = null,

        int pageIndex = 0, int pageSize = int.MaxValue);

    Task<AcademicYearGradeSectionStudentMapping> GetAcademicYearGradeSectionStudentMappingByIdAsync(int id);

    Task<IList<AcademicYearGradeSectionStudentMapping>> GetAcademicYearGradeSectionStudentMappingsByIdsAsync(IEnumerable<int> ids);

    Task InsertAcademicYearGradeSectionStudentMappingAsync(AcademicYearGradeSectionStudentMapping academicYearGradeSectionStudentMapping);
    
    Task InsertAcademicYearGradeSectionStudentMappingAsync(IEnumerable<AcademicYearGradeSectionStudentMapping> academicYearGradeSectionStudentMappings);

    Task UpdateAcademicYearGradeSectionStudentMappingAsync(AcademicYearGradeSectionStudentMapping academicYearGradeSectionStudentMapping);

    Task UpdateAcademicYearGradeSectionStudentMappingAsync(IEnumerable<AcademicYearGradeSectionStudentMapping> academicYearGradeSectionStudentMappings);

    Task DeleteAcademicYearGradeSectionStudentMappingAsync(AcademicYearGradeSectionStudentMapping academicYearGradeSectionStudentMapping);

    Task DeleteAcademicYearGradeSectionStudentMappingAsync(IEnumerable<AcademicYearGradeSectionStudentMapping> academicYearGradeSectionStudentMappings);
}