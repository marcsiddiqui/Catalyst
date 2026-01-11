using Nop.Core;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.HolidaysNEvents;

namespace Nop.Services.HolidaysNEvents;

public partial interface IHolidayService
{
    Task<IPagedList<Holiday>> GetAllHolidaysAsync(
        int id = 0, IEnumerable<int> ids = null,
        int academicYearId = 0, IEnumerable<int> academicYearIds = null,
        string name = null, IEnumerable<string> names = null,
        int storeId = 0, IEnumerable<int> storeIds = null,


        BooleanFilter deleted = BooleanFilter.False,
        BooleanFilter limitedToStores = BooleanFilter.Both,

        int pageIndex = 0, int pageSize = int.MaxValue);

    Task<Holiday> GetHolidayByIdAsync(int id);

    Task<IList<Holiday>> GetHolidaysByIdsAsync(IEnumerable<int> ids);

    Task InsertHolidayAsync(Holiday holiday);
    
    Task InsertHolidayAsync(IEnumerable<Holiday> holidays);

    Task UpdateHolidayAsync(Holiday holiday);

    Task UpdateHolidayAsync(IEnumerable<Holiday> holidays);

    Task DeleteHolidayAsync(Holiday holiday);

    Task DeleteHolidayAsync(IEnumerable<Holiday> holidays);
}