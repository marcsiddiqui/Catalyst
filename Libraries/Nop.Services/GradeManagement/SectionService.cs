using Nop.Core;
using Nop.Core.Domain.GradeManagement;
using Nop.Data;

namespace Nop.Services.GradeManagement;

public partial class SectionService : ISectionService
{
    #region Fields

    protected readonly IRepository<Section> _sectionRepository;

    #endregion

    #region Ctor

    public SectionService(
        IRepository<Section> sectionRepository
        )
    {
        _sectionRepository = sectionRepository;
    }

    #endregion

    #region Methods

    public virtual async Task<IPagedList<Section>> GetAllSectionsAsync(
        int id = 0, IEnumerable<int> ids = null,
        string name = null, IEnumerable<string> names = null,
        BooleanFilter deleted = BooleanFilter.False,



        int pageIndex = 0, int pageSize = int.MaxValue)
    {
        var productReviews = await _sectionRepository.GetAllPagedAsync(async query =>
        {
            if (id > 0)
                query = query.Where(x => x.Id == id);

            if (ids != null && ids.Any())
                query = query.Where(x => ids.Contains(x.Id));

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(x => name == x.Name);

            if (names != null && names.Any())
                query = query.Where(x => names.Contains(x.Name));

            query = query.WhereBoolean(x => x.Deleted, deleted);





            return query;

        }, pageIndex, pageSize);

        return productReviews;
    }

    public virtual async Task<Section> GetSectionByIdAsync(int id)
    {
        if (id <= 0)
            return null;

        return await _sectionRepository.GetByIdAsync(id);
    }

    public virtual async Task<IList<Section>> GetSectionsByIdsAsync(IEnumerable<int> ids)
    {
        if (ids == null || !ids.Any())
            return null;

        return await _sectionRepository.GetByIdsAsync(ids.ToList());
    }

    public virtual async Task InsertSectionAsync(Section section)
    {
        if (section == null)
            return;

        await _sectionRepository.InsertAsync(section);
    }
    
    public virtual async Task InsertSectionAsync(IEnumerable<Section> sections)
    {
        if (sections == null || !sections.Any())
            return;

        await _sectionRepository.InsertAsync(sections.ToList());
    }

    public virtual async Task UpdateSectionAsync(Section section)
    {
        if (section == null)
            return;

        await _sectionRepository.UpdateAsync(section);
    }

    public virtual async Task UpdateSectionAsync(IEnumerable<Section> sections)
    {
        if (sections == null || !sections.Any())
            return;

        await _sectionRepository.UpdateAsync(sections.ToList());
    }

    public virtual async Task DeleteSectionAsync(Section section)
    {
        if (section == null)
            return;

        await _sectionRepository.DeleteAsync(section);
    }

    public virtual async Task DeleteSectionAsync(IEnumerable<Section> sections)
    {
        if (sections == null || !sections.Any())
            return;

        await _sectionRepository.DeleteAsync(sections.ToList());
    }

    #endregion
}