using Nop.Core.Domain.HolidaysNEvents;
using Nop.Services.AcademicYears;
using Nop.Services.HolidaysNEvents;
using Nop.Services.Localization;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.HolidaysNEvents;
using Nop.Web.Framework.Factories;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Web.Areas.Admin.Factories;

public partial class EventModelFactory : IEventModelFactory
{
    #region Fields

    protected readonly IEventService _eventService;
    protected readonly ILocalizationService _localizationService;
    protected readonly ILocalizedModelFactory _localizedModelFactory;
    protected readonly IStoreMappingSupportedModelFactory _storeMappingSupportedModelFactory;
    protected readonly IBaseAdminModelFactory _baseAdminModelFactory;
    protected readonly IAcademicYearService _academicYearService;

    #endregion

    #region Ctor

    public EventModelFactory(
        IEventService eventService,
        ILocalizationService localizationService,
        ILocalizedModelFactory localizedModelFactory,
        IStoreMappingSupportedModelFactory storeMappingSupportedModelFactory,
        IBaseAdminModelFactory baseAdminModelFactory,
        IAcademicYearService academicYearService)
    {
        _eventService = eventService;
        _localizationService = localizationService;
        _localizedModelFactory = localizedModelFactory;
        _storeMappingSupportedModelFactory = storeMappingSupportedModelFactory;
        _baseAdminModelFactory = baseAdminModelFactory;
        _academicYearService = academicYearService;
    }

    #endregion

    #region Utilities



    #endregion

    #region Methods

    public virtual async Task<EventSearchModel> PrepareEventSearchModelAsync(EventSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        //prepare available stores
        await _baseAdminModelFactory.PrepareStoresAsync(searchModel.AvailableStores);
        await _baseAdminModelFactory.PrepareAvailableYearsAsync(searchModel.AvailableYears);

        //prepare page parameters
        searchModel.SetGridPageSize();

        return searchModel;
    }

    public virtual async Task<EventListModel> PrepareEventListModelAsync(EventSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        //get events
        var events = await _eventService.GetAllEventsAsync(
            storeId: searchModel.SearchStoreId,
            pageIndex: searchModel.Page - 1,
            pageSize: searchModel.PageSize);

        //prepare list model
        var model = await new EventListModel().PrepareToGridAsync(searchModel, events, () =>
        {
            //fill in model values from the entity
            return events.SelectAwait(async @event =>
            {
                var eventModel = @event.ToModel<EventModel>();

                var academicYear = await _academicYearService.GetAcademicYearByIdAsync(@event.AcademicYearId);
                eventModel.AcademicYear = academicYear?.Name;

                return eventModel;
            });
        });

        return model;
    }

    public virtual async Task<EventModel> PrepareEventModelAsync(EventModel model, Event @event, bool excludeProperties = false)
    {
        Func<EventLocalizedModel, int, Task> localizedModelConfiguration = null;

        if (@event != null)
        {
            //fill in model values from the entity
            if (model == null)
            {
                model = @event.ToModel<EventModel>();
            }

            //define localized model configuration action
            localizedModelConfiguration = async (locale, languageId) =>
            {
                locale.Name = await _localizationService.GetLocalizedAsync(@event, entity => entity.Name, languageId, false, false);
            };
        }

        //set default values for the new model
        if (@event == null)
        {

        }

        //prepare localized models
        if (!excludeProperties)
            model.Locales = await _localizedModelFactory.PrepareLocalizedModelsAsync(localizedModelConfiguration);


        //prepare available stores
        await _storeMappingSupportedModelFactory.PrepareModelStoresAsync(model, @event, excludeProperties);

        await _baseAdminModelFactory.PrepareAvailableYearsAsync(model.AvailableYears, defaultItemText: await _localizationService.GetResourceAsync("admin.common.select"));

        return model;
    }

    #endregion
}