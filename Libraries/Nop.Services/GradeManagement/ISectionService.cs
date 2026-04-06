using Nop.Core;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.GradeManagement;

namespace Nop.Services.GradeManagement;

public partial interface ISectionService
{
    Task<IPagedList<Section>> GetAllSectionsAsync(
        int id = 0, IEnumerable<int> ids = null,
        string name = null, IEnumerable<string> names = null,
        BooleanFilter deleted = BooleanFilter.False,



        int pageIndex = 0, int pageSize = int.MaxValue);

    Task<Section> GetSectionByIdAsync(int id);

    Task<IList<Section>> GetSectionsByIdsAsync(IEnumerable<int> ids);

    Task InsertSectionAsync(Section section);
    
    Task InsertSectionAsync(IEnumerable<Section> sections);

    Task UpdateSectionAsync(Section section);

    Task UpdateSectionAsync(IEnumerable<Section> sections);

    Task DeleteSectionAsync(Section section);

    Task DeleteSectionAsync(IEnumerable<Section> sections);
}