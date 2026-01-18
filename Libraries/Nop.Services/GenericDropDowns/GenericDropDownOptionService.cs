using Nop.Core;
using Nop.Core.Domain.GenericDropDowns;
using Nop.Data;

namespace Nop.Services.GenericDropDowns;

public partial class GenericDropDownOptionService : IGenericDropDownOptionService
{
    #region Fields

    protected readonly IRepository<GenericDropDownOption> _genericDropDownOptionRepository;

    #endregion

    #region Ctor

    public GenericDropDownOptionService(
        IRepository<GenericDropDownOption> genericDropDownOptionRepository
        )
    {
        _genericDropDownOptionRepository = genericDropDownOptionRepository;
    }

    #endregion

    #region Methods

    public virtual async Task<IPagedList<GenericDropDownOption>> GetAllGenericDropDownOptionsAsync(
        int id = 0, IEnumerable<int> ids = null,
        GenericDropdownEntity? entity = null, IEnumerable<GenericDropdownEntity> entities = null,
        string text = null, IEnumerable<string> texts = null,


        string color = null, IEnumerable<string> colors = null,
        BooleanFilter isSystemOption = BooleanFilter.Both,

        int pageIndex = 0, int pageSize = int.MaxValue)
    {
        var productReviews = await _genericDropDownOptionRepository.GetAllPagedAsync(async query =>
        {
            if (id > 0)
                query = query.Where(x => x.Id == id);

            if (ids != null && ids.Any())
                query = query.Where(x => ids.Contains(x.Id));

            if (entity.HasValue)
                query = query.Where(x => x.EntityId == (int)entity.Value);

            if (entities != null && entities.Any())
                query = query.Where(x => entities.Contains((GenericDropdownEntity)x.EntityId));

            if (!string.IsNullOrWhiteSpace(text))
                query = query.Where(x => text == x.Text);

            if (texts != null && texts.Any())
                query = query.Where(x => texts.Contains(x.Text));



            if (!string.IsNullOrWhiteSpace(color))
                query = query.Where(x => color == x.Color);

            if (colors != null && colors.Any())
                query = query.Where(x => colors.Contains(x.Color));

            query = query.WhereBoolean(x => x.IsSystemOption, isSystemOption);



            return query;

        }, pageIndex, pageSize);

        return productReviews;
    }

    public virtual async Task<GenericDropDownOption> GetGenericDropDownOptionByIdAsync(int id)
    {
        if (id <= 0)
            return null;

        return await _genericDropDownOptionRepository.GetByIdAsync(id);
    }

    public virtual async Task<IList<GenericDropDownOption>> GetGenericDropDownOptionsByIdsAsync(IEnumerable<int> ids)
    {
        if (ids == null || !ids.Any())
            return null;

        return await _genericDropDownOptionRepository.GetByIdsAsync(ids.ToList());
    }

    public virtual async Task InsertGenericDropDownOptionAsync(GenericDropDownOption genericDropDownOption)
    {
        if (genericDropDownOption == null)
            return;

        await _genericDropDownOptionRepository.InsertAsync(genericDropDownOption);
    }
    
    public virtual async Task InsertGenericDropDownOptionAsync(IEnumerable<GenericDropDownOption> genericDropDownOptions)
    {
        if (genericDropDownOptions == null || !genericDropDownOptions.Any())
            return;

        await _genericDropDownOptionRepository.InsertAsync(genericDropDownOptions.ToList());
    }

    public virtual async Task UpdateGenericDropDownOptionAsync(GenericDropDownOption genericDropDownOption)
    {
        if (genericDropDownOption == null)
            return;

        await _genericDropDownOptionRepository.UpdateAsync(genericDropDownOption);
    }

    public virtual async Task UpdateGenericDropDownOptionAsync(IEnumerable<GenericDropDownOption> genericDropDownOptions)
    {
        if (genericDropDownOptions == null || !genericDropDownOptions.Any())
            return;

        await _genericDropDownOptionRepository.UpdateAsync(genericDropDownOptions.ToList());
    }

    public virtual async Task DeleteGenericDropDownOptionAsync(GenericDropDownOption genericDropDownOption)
    {
        if (genericDropDownOption == null)
            return;

        await _genericDropDownOptionRepository.DeleteAsync(genericDropDownOption);
    }

    public virtual async Task DeleteGenericDropDownOptionAsync(IEnumerable<GenericDropDownOption> genericDropDownOptions)
    {
        if (genericDropDownOptions == null || !genericDropDownOptions.Any())
            return;

        await _genericDropDownOptionRepository.DeleteAsync(genericDropDownOptions.ToList());
    }



    #endregion
}