using Nop.Core.Domain.AcademicYears;
using Nop.Core.Domain.HolidaysNEvents;
using Nop.Services;
using Nop.Services.AcademicYears;
using Nop.Services.HolidaysNEvents;
using Nop.Services.Localization;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.HolidaysNEvents;
using Nop.Web.Framework.Factories;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Web.Areas.Admin.Factories;

public partial class HolidayModelFactory : IHolidayModelFactory
{
    #region Fields

    protected readonly IHolidayService _holidayService;
    protected readonly ILocalizationService _localizationService;
    protected readonly ILocalizedModelFactory _localizedModelFactory;
    protected readonly IStoreMappingSupportedModelFactory _storeMappingSupportedModelFactory;
    protected readonly IAcademicYearService _academicYearService;

    #endregion

    #region Ctor

    public HolidayModelFactory(
        IHolidayService holidayService,
        ILocalizationService localizationService,
        ILocalizedModelFactory localizedModelFactory,
        IStoreMappingSupportedModelFactory storeMappingSupportedModelFactory,
        IAcademicYearService academicYearService)
    {
        _holidayService = holidayService;
        _localizationService = localizationService;
        _localizedModelFactory = localizedModelFactory;
        _storeMappingSupportedModelFactory = storeMappingSupportedModelFactory;
        _academicYearService = academicYearService;
    }

    #endregion

    #region Utilities



    #endregion

    #region Methods

    public virtual Task<HolidaySearchModel> PrepareHolidaySearchModelAsync(HolidaySearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        //prepare page parameters
        searchModel.SetGridPageSize();

        return Task.FromResult(searchModel);
    }

    public virtual async Task<HolidayListModel> PrepareHolidayListModelAsync(HolidaySearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        //get holidays
        var holidays = await _holidayService.GetAllHolidaysAsync(
            pageIndex: searchModel.Page - 1,
            pageSize: searchModel.PageSize);

        //prepare list model
        var model = await new HolidayListModel().PrepareToGridAsync(searchModel, holidays, () =>
        {
            //fill in model values from the entity
            return holidays.SelectAwait(async holiday =>
            {
                var holidayModel = holiday.ToModel<HolidayModel>();

                return holidayModel;
            });
        });

        return model;
    }

    public virtual async Task<HolidayModel> PrepareHolidayModelAsync(HolidayModel model, Holiday holiday, bool excludeProperties = false)
    {
        Func<HolidayLocalizedModel, int, Task> localizedModelConfiguration = null;

        if (holiday != null)
        {
            //fill in model values from the entity
            if (model == null)
            {
                model = holiday.ToModel<HolidayModel>();
            }

            //define localized model configuration action
            localizedModelConfiguration = async (locale, languageId) =>
            {
                locale.Name = await _localizationService.GetLocalizedAsync(holiday, entity => entity.Name, languageId, false, false);
            };
        }

        //set default values for the new model
        if (holiday == null)
        {

        }

        //prepare localized models
        if (!excludeProperties)
            model.Locales = await _localizedModelFactory.PrepareLocalizedModelsAsync(localizedModelConfiguration);


        //prepare available stores
        await _storeMappingSupportedModelFactory.PrepareModelStoresAsync(model, holiday, excludeProperties);

        model.AvailableYears = await (await _academicYearService.GetAllAcademicYearsAsync())
            .ToSelectList(x => (x as AcademicYear).Name)
            .ToListAsync();


        return model;
    }

    #endregion
}