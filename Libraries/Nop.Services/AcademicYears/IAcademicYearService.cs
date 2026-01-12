using Nop.Core;
using Nop.Core.Domain.AcademicYears;
using Nop.Core.Domain.Common;

namespace Nop.Services.AcademicYears;

public partial interface IAcademicYearService
{
    #region AcademicYear

    Task<IPagedList<AcademicYear>> GetAllAcademicYearsAsync(
        int id = 0, IEnumerable<int> ids = null,
        string name = null, IEnumerable<string> names = null,


        BooleanFilter deleted = BooleanFilter.False,

        int pageIndex = 0, int pageSize = int.MaxValue);

    Task<AcademicYear> GetAcademicYearByIdAsync(int id);

    Task<IList<AcademicYear>> GetAcademicYearsByIdsAsync(IEnumerable<int> ids);

    Task InsertAcademicYearAsync(AcademicYear academicYear);
    
    Task InsertAcademicYearAsync(IEnumerable<AcademicYear> academicYears);

    Task UpdateAcademicYearAsync(AcademicYear academicYear);

    Task UpdateAcademicYearAsync(IEnumerable<AcademicYear> academicYears);

    Task DeleteAcademicYearAsync(AcademicYear academicYear);

    Task DeleteAcademicYearAsync(IEnumerable<AcademicYear> academicYears);

    #endregion

    #region AcademicYearGradeSectionMapping

    Task<IPagedList<AcademicYearGradeSectionMapping>> GetAllAcademicYearGradeSectionMappingsAsync(
        int id = 0, IEnumerable<int> ids = null,
        int academicYearId = 0, IEnumerable<int> academicYearIds = null,
        int gradeId = 0, IEnumerable<int> gradeIds = null,
        int sectionId = 0, IEnumerable<int> sectionIds = null,


        int pageIndex = 0, int pageSize = int.MaxValue);

    Task<AcademicYearGradeSectionMapping> GetAcademicYearGradeSectionMappingByIdAsync(int id);

    Task<IList<AcademicYearGradeSectionMapping>> GetAcademicYearGradeSectionMappingsByIdsAsync(IEnumerable<int> ids);

    Task InsertAcademicYearGradeSectionMappingAsync(AcademicYearGradeSectionMapping academicYearGradeSectionMapping);

    Task InsertAcademicYearGradeSectionMappingAsync(IEnumerable<AcademicYearGradeSectionMapping> academicYearGradeSectionMappings);

    Task UpdateAcademicYearGradeSectionMappingAsync(AcademicYearGradeSectionMapping academicYearGradeSectionMapping);

    Task UpdateAcademicYearGradeSectionMappingAsync(IEnumerable<AcademicYearGradeSectionMapping> academicYearGradeSectionMappings);

    Task DeleteAcademicYearGradeSectionMappingAsync(AcademicYearGradeSectionMapping academicYearGradeSectionMapping);

    Task DeleteAcademicYearGradeSectionMappingAsync(IEnumerable<AcademicYearGradeSectionMapping> academicYearGradeSectionMappings);

    #endregion

    #region AcademicYearTerm

    Task<IPagedList<AcademicYearTerm>> GetAllAcademicYearTermsAsync(
        int id = 0, IEnumerable<int> ids = null,
        int academicYearGradeSectionMappingId = 0, IEnumerable<int> academicYearGradeSectionMappingIds = null,

        string name = null, IEnumerable<string> names = null,


        BooleanFilter deleted = BooleanFilter.False,

        int pageIndex = 0, int pageSize = int.MaxValue);

    Task<AcademicYearTerm> GetAcademicYearTermByIdAsync(int id);

    Task<IList<AcademicYearTerm>> GetAcademicYearTermsByIdsAsync(IEnumerable<int> ids);

    Task InsertAcademicYearTermAsync(AcademicYearTerm academicYearTerm);

    Task InsertAcademicYearTermAsync(IEnumerable<AcademicYearTerm> academicYearTerms);

    Task UpdateAcademicYearTermAsync(AcademicYearTerm academicYearTerm);

    Task UpdateAcademicYearTermAsync(IEnumerable<AcademicYearTerm> academicYearTerms);

    Task DeleteAcademicYearTermAsync(AcademicYearTerm academicYearTerm);

    Task DeleteAcademicYearTermAsync(IEnumerable<AcademicYearTerm> academicYearTerms);

    #endregion
}