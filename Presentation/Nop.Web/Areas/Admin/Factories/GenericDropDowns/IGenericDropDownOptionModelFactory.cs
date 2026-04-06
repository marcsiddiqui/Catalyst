using Nop.Core.Domain.GenericDropDowns;
using Nop.Web.Areas.Admin.Models.GenericDropDowns;

namespace Nop.Web.Areas.Admin.Factories;

public partial interface IGenericDropDownOptionModelFactory
{
    Task<GenericDropDownOptionSearchModel> PrepareGenericDropDownOptionSearchModelAsync(GenericDropDownOptionSearchModel searchModel);

    Task<GenericDropDownOptionListModel> PrepareGenericDropDownOptionListModelAsync(GenericDropDownOptionSearchModel searchModel);

    Task<GenericDropDownOptionModel> PrepareGenericDropDownOptionModelAsync(GenericDropDownOptionModel model, GenericDropDownOption genericDropDownOption, bool excludeProperties = false);
}