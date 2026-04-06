using Nop.Core.Domain.HolidaysNEvents;
using Nop.Web.Areas.Admin.Models.HolidaysNEvents;

namespace Nop.Web.Areas.Admin.Factories;

public partial interface IHolidayModelFactory
{
    Task<HolidaySearchModel> PrepareHolidaySearchModelAsync(HolidaySearchModel searchModel);

    Task<HolidayListModel> PrepareHolidayListModelAsync(HolidaySearchModel searchModel);

    Task<HolidayModel> PrepareHolidayModelAsync(HolidayModel model, Holiday holiday, bool excludeProperties = false);
}