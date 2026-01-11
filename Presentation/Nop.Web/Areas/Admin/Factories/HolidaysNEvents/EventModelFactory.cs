using Nop.Core.Domain.HolidaysNEvents;
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

    #endregion

    #region Ctor

    public EventModelFactory(IEventService eventService,
        ILocalizationService localizationService,
        ILocalizedModelFactory localizedModelFactory,
        IStoreMappingSupportedModelFactory storeMappingSupportedModelFactory)
    {
        _eventService = eventService;
        _localizationService = localizationService;
        _localizedModelFactory = localizedModelFactory;
        _storeMappingSupportedModelFactory = storeMappingSupportedModelFactory;
    }

    #endregion

    #region Utilities

    

    #endregion

    #region Methods

    public virtual Task<EventSearchModel> PrepareEventSearchModelAsync(EventSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        //prepare page parameters
        searchModel.SetGridPageSize();

        return Task.FromResult(searchModel);
    }

    public virtual async Task<EventListModel> PrepareEventListModelAsync(EventSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        //get events
        var events = (await _eventService.GetAllEventsAsync()).ToPagedList(searchModel);

        //prepare list model
        var model = await new EventListModel().PrepareToGridAsync(searchModel, events, () =>
        {
            //fill in model values from the entity
            return events.SelectAwait(async @event =>
            {
                var eventModel = @event.ToModel<EventModel>();

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


        return model;
    }

    #endregion
}