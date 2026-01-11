using Nop.Core;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.AcademicYears;

namespace Nop.Services.AcademicYears;

public partial interface IAcademicYearService
{
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
}