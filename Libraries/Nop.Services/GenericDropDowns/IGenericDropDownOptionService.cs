using Nop.Core;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.GenericDropDowns;

namespace Nop.Services.GenericDropDowns;

public partial interface IGenericDropDownOptionService
{
    Task<IPagedList<GenericDropDownOption>> GetAllGenericDropDownOptionsAsync(
        int id = 0, IEnumerable<int> ids = null,
        int entityId = 0, IEnumerable<int> entityIds = null,
        string text = null, IEnumerable<string> texts = null,


        string color = null, IEnumerable<string> colors = null,
        BooleanFilter isSystemOption = BooleanFilter.Both,

        int pageIndex = 0, int pageSize = int.MaxValue);

    Task<GenericDropDownOption> GetGenericDropDownOptionByIdAsync(int id);

    Task<IList<GenericDropDownOption>> GetGenericDropDownOptionsByIdsAsync(IEnumerable<int> ids);

    Task InsertGenericDropDownOptionAsync(GenericDropDownOption genericDropDownOption);
    
    Task InsertGenericDropDownOptionAsync(IEnumerable<GenericDropDownOption> genericDropDownOptions);

    Task UpdateGenericDropDownOptionAsync(GenericDropDownOption genericDropDownOption);

    Task UpdateGenericDropDownOptionAsync(IEnumerable<GenericDropDownOption> genericDropDownOptions);

    Task DeleteGenericDropDownOptionAsync(GenericDropDownOption genericDropDownOption);

    Task DeleteGenericDropDownOptionAsync(IEnumerable<GenericDropDownOption> genericDropDownOptions);
}