using DocumentFormat.OpenXml.Vml.Spreadsheet;
using Nop.Core;
using Nop.Core.Domain.HolidaysNEvents;
using Nop.Data;
using Nop.Services.Stores;

namespace Nop.Services.HolidaysNEvents;

public partial class HolidayService : IHolidayService
{
    #region Fields

    protected readonly IRepository<Holiday> _holidayRepository;
    protected readonly IStoreMappingService _storeMappingService;

    #endregion

    #region Ctor

    public HolidayService(
        IRepository<Holiday> holidayRepository,
        IStoreMappingService storeMappingService)
    {
        _holidayRepository = holidayRepository;
        _storeMappingService = storeMappingService;
    }

    #endregion

    #region Methods

    public virtual async Task<IPagedList<Holiday>> GetAllHolidaysAsync(
        IEnumerable<int> academicYearIds = default,
        string name = default,
        int storeId = default,
        bool showHidden = false,
        int pageIndex = default,
        int pageSize = int.MaxValue)
    {
        var productReviews = await _holidayRepository.GetAllPagedAsync(async query =>
        {
            if (academicYearIds != null)
                query = query.Where(x => academicYearIds.Contains(x.AcademicYearId));

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(x => name == x.Name);

            if (!showHidden || storeId > 0)
            {
                //apply store mapping constraints
                query = await _storeMappingService.ApplyStoreMapping(query, storeId);
            }

            return query;

        }, pageIndex, pageSize, includeDeleted: false);

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