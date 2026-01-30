using Nop.Core;
using Nop.Core.Domain.HolidaysNEvents;
using Nop.Data;

namespace Nop.Services.HolidaysNEvents;

public partial class HolidayService : IHolidayService
{
    #region Fields

    protected readonly IRepository<Holiday> _holidayRepository;

    #endregion

    #region Ctor

    public HolidayService(
        IRepository<Holiday> holidayRepository
        )
    {
        _holidayRepository = holidayRepository;
    }

    #endregion

    #region Methods

    public virtual async Task<IPagedList<Holiday>> GetAllHolidaysAsync(
        int id = 0, IEnumerable<int> ids = null,
        int academicYearId = 0, IEnumerable<int> academicYearIds = null,
        string name = null, IEnumerable<string> names = null,
        int storeId = 0, IEnumerable<int> storeIds = null,
        BooleanFilter deleted = BooleanFilter.False,
        BooleanFilter limitedToStores = BooleanFilter.Both,
        int pageIndex = 0, int pageSize = int.MaxValue)
    {
        var productReviews = await _holidayRepository.GetAllPagedAsync(async query =>
        {
            if (id > 0)
                query = query.Where(x => x.Id == id);

            if (ids != null && ids.Any())
                query = query.Where(x => ids.Contains(x.Id));

            if (academicYearId > 0)
                query = query.Where(x => academicYearId == x.AcademicYearId);

            if (academicYearIds != null && academicYearIds.Any())
                query = query.Where(x => academicYearIds.Contains(x.AcademicYearId));

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(x => name == x.Name);

            if (names != null && names.Any())
                query = query.Where(x => names.Contains(x.Name));

            if (storeId > 0)
                query = query.Where(x => storeId == x.StoreId);

            if (storeIds != null && storeIds.Any())
                query = query.Where(x => storeIds.Contains(x.StoreId));

            query = query.WhereBoolean(x => x.Deleted, deleted);

            query = query.WhereBoolean(x => x.LimitedToStores, limitedToStores);

            return query;

        }, pageIndex, pageSize);

        return productReviews;
    }

    public virtual async Task<Holiday> GetHolidayByIdAsync(int id)
    {
        if (id <= 0)
            return null;

        return await _holidayRepository.GetByIdAsync(id);
    }

    public virtual async Task<IList<Holiday>> GetHolidaysByIdsAsync(IEnumerable<int> ids)
    {
        if (ids == null || !ids.Any())
            return null;

        return await _holidayRepository.GetByIdsAsync(ids.ToList());
    }

    public virtual async Task InsertHolidayAsync(Holiday holiday)
    {
        if (holiday == null)
            return;

        await _holidayRepository.InsertAsync(holiday);
    }
    
    public virtual async Task InsertHolidayAsync(IEnumerable<Holiday> holidays)
    {
        if (holidays == null || !holidays.Any())
            return;

        await _holidayRepository.InsertAsync(holidays.ToList());
    }

    public virtual async Task UpdateHolidayAsync(Holiday holiday)
    {
        if (holiday == null)
            return;

        await _holidayRepository.UpdateAsync(holiday);
    }

    public virtual async Task UpdateHolidayAsync(IEnumerable<Holiday> holidays)
    {
        if (holidays == null || !holidays.Any())
            return;

        await _holidayRepository.UpdateAsync(holidays.ToList());
    }

    public virtual async Task DeleteHolidayAsync(Holiday holiday)
    {
        if (holiday == null)
            return;

        await _holidayRepository.DeleteAsync(holiday);
    }

    public virtual async Task DeleteHolidayAsync(IEnumerable<Holiday> holidays)
    {
        if (holidays == null || !holidays.Any())
            return;

        await _holidayRepository.DeleteAsync(holidays.ToList());
    }

    #endregion
}