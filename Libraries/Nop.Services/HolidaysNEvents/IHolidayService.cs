using Nop.Core;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.HolidaysNEvents;

namespace Nop.Services.HolidaysNEvents;

public partial interface IHolidayService
{
    Task<IPagedList<Holiday>> GetAllHolidaysAsync(
        IEnumerable<int> academicYearIds = default,
        string name = default,
        int storeId = default,
        bool showHidden = false,
        int pageIndex = default,
        int pageSize = int.MaxValue);

    Task<Holiday> GetHolidayByIdAsync(int id);

    Task<IList<Holiday>> GetHolidaysByIdsAsync(IEnumerable<int> ids);

    Task InsertHolidayAsync(Holiday holiday);

    Task InsertHolidayAsync(IEnumerable<Holiday> holidays);

    Task UpdateHolidayAsync(Holiday holiday);

    Task UpdateHolidayAsync(IEnumerable<Holiday> holidays);

    Task DeleteHolidayAsync(Holiday holiday);

    Task DeleteHolidayAsync(IEnumerable<Holiday> holidays);
}