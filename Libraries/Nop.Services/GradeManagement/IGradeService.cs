using Nop.Core;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.GradeManagement;

namespace Nop.Services.GradeManagement;

public partial interface IGradeService
{
    #region Grade

    Task<IPagedList<Grade>> GetAllGradesAsync(
        int id = 0, IEnumerable<int> ids = null,
        string name = null, IEnumerable<string> names = null,
        BooleanFilter isActive = BooleanFilter.True,
        BooleanFilter deleted = BooleanFilter.False,
        int storeId = 0, IEnumerable<int> storeIds = null,


        BooleanFilter limitedToStores = BooleanFilter.Both,

        int pageIndex = 0, int pageSize = int.MaxValue);

    Task<Grade> GetGradeByIdAsync(int id);

    Task<IList<Grade>> GetGradesByIdsAsync(IEnumerable<int> ids);

    Task InsertGradeAsync(Grade grade);
    
    Task InsertGradeAsync(IEnumerable<Grade> grades);

    Task UpdateGradeAsync(Grade grade);

    Task UpdateGradeAsync(IEnumerable<Grade> grades);

    Task DeleteGradeAsync(Grade grade);

    Task DeleteGradeAsync(IEnumerable<Grade> grades);

    #endregion

    #region Methods

    Task<IPagedList<GradeSubjectMapping>> GetAllGradeSubjectMappingsAsync(
        int id = 0, IEnumerable<int> ids = null,
        int gradeId = 0, IEnumerable<int> gradeIds = null,
        int subjectId = 0, IEnumerable<int> subjectIds = null,
        int sectionId = 0, IEnumerable<int> sectionIds = null,
        BooleanFilter deleted = BooleanFilter.False,
        int pageIndex = 0, int pageSize = int.MaxValue);

    Task<GradeSubjectMapping> GetGradeSubjectMappingByIdAsync(int id);

    Task<IList<GradeSubjectMapping>> GetGradeSubjectMappingsByIdsAsync(IEnumerable<int> ids);

    Task InsertGradeSubjectMappingAsync(GradeSubjectMapping gradeSubjectMapping);

    Task InsertGradeSubjectMappingAsync(IEnumerable<GradeSubjectMapping> gradeSubjectMappings);

    Task UpdateGradeSubjectMappingAsync(GradeSubjectMapping gradeSubjectMapping);

    Task UpdateGradeSubjectMappingAsync(IEnumerable<GradeSubjectMapping> gradeSubjectMappings);

    Task DeleteGradeSubjectMappingAsync(GradeSubjectMapping gradeSubjectMapping);

    Task DeleteGradeSubjectMappingAsync(IEnumerable<GradeSubjectMapping> gradeSubjectMappings);

    #endregion
}