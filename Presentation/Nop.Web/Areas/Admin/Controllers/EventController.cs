using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.HolidaysNEvents;
using Nop.Services.HolidaysNEvents;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Services.Stores;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.HolidaysNEvents;
using Nop.Web.Framework.Mvc;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Web.Areas.Admin.Controllers;

public partial class EventController : BaseAdminController
{
    #region Fields

    protected readonly IEventModelFactory _eventModelFactory;
    protected readonly IEventService _eventService;
    protected readonly ICustomerActivityService _customerActivityService;
    protected readonly ILocalizationService _localizationService;
    protected readonly ILocalizedEntityService _localizedEntityService;
    protected readonly INotificationService _notificationService;
    protected readonly IStoreMappingService _storeMappingService;

    #endregion

    #region Ctor

    public EventController(
        IEventModelFactory eventModelFactory,
        IEventService eventService,
        ICustomerActivityService customerActivityService,
        ILocalizationService localizationService,
        ILocalizedEntityService localizedEntityService,
        INotificationService notificationService,
        IStoreMappingService storeMappingService)
    {
        _eventModelFactory = eventModelFactory;
        _eventService = eventService;
        _customerActivityService = customerActivityService;
        _localizationService = localizationService;
        _localizedEntityService = localizedEntityService;
        _notificationService = notificationService;
        _storeMappingService = storeMappingService;
    }

    #endregion

    #region Utilities

    protected virtual async Task UpdateLocalesAsync(Event @event, EventModel model)
    {
        foreach (var localized in model.Locales)
        {
            await _localizedEntityService.SaveLocalizedValueAsync(@event,
                x => x.Name,
                localized.Name,
                localized.LanguageId);


        }
    }

    #endregion

    #region Academic year grade section event mappings

    [CheckPermission(StandardPermission.HolidaysNEvents.MANAGE_EVENTS)]
    public virtual async Task<IActionResult> AddOrEditAcademicYearGradeSectionEventMappingPopup(int eventId, int mappingId = 0)
    {
        var mapping = mappingId > 0
            ? await _eventService.GetAcademicYearGradeSectionEventMappingByIdAsync(mappingId)
            : null;

        if (mapping != null && mapping.EventId != eventId)
            return RedirectToAction("List");

        var model = await _eventModelFactory.PrepareAcademicYearGradeSectionEventMappingModelAsync(
            mapping == null
                ? new AcademicYearGradeSectionEventMappingModel { EventId = eventId }
                : null,
            mapping);

        return View(model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.HolidaysNEvents.MANAGE_EVENTS)]
    public virtual async Task<IActionResult> AcademicYearGradeSectionEventMappingGrid(AcademicYearGradeSectionEventMappingSearchModel searchModel)
    {
        var model = await _eventModelFactory.PrepareAcademicYearGradeSectionEventMappingListModelAsync(searchModel);

        return Json(model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.HolidaysNEvents.MANAGE_EVENTS)]
    public virtual async Task<IActionResult> AcademicYearGradeSectionEventMappingSave(AcademicYearGradeSectionEventMappingModel model)
    {
        if (model.EventId <= 0)
            return Json(new { Result = "error", Errors = new[] { "Event is required." } });

        if (model.Id > 0 && model.AcademicYearClassSectionMappingId <= 0)
            return Json(new { Result = "error", Errors = new[] { "Class - Section - Academic Year is required." } });

        if (model.Id == 0)
        {
            var selectedMappingIds = model.SelectedAcademicYearClassSectionMappingIds?
                .Where(mappingId => mappingId > 0)
                .Distinct()
                .ToList() ?? new List<int>();

            if (!selectedMappingIds.Any())
                return Json(new { Result = "error", Errors = new[] { "Class - Section - Academic Year is required." } });

            var existingMappings = await _eventService.GetAllAcademicYearGradeSectionEventMappingsAsync(
                eventId: model.EventId,
                academicYearGradeSectionMappingIds: selectedMappingIds);

            if (existingMappings.Any())
                return Json(new { Result = "error", Errors = new[] { "One or more selected Class - Section - Academic Year mappings are already mapped to the event." } });

            var mappings = selectedMappingIds.Select(mappingId => new AcademicYearGradeSectionEventMapping
            {
                EventId = model.EventId,
                AcademicYearGradeSectionMappingId = mappingId,
                Amount = model.Amount
            });

            await _eventService.InsertAcademicYearGradeSectionEventMappingAsync(mappings);

            return Json(new { Result = "success" });
        }

        var duplicateMappings = await _eventService.GetAllAcademicYearGradeSectionEventMappingsAsync(
            eventId: model.EventId,
            academicYearGradeSectionMappingId: model.AcademicYearClassSectionMappingId);

        if (duplicateMappings.Any(mapping => mapping.Id != model.Id))
            return Json(new { Result = "error", Errors = new[] { "This Class - Section - Academic Year is already mapped to the event." } });

        var mapping = await _eventService.GetAcademicYearGradeSectionEventMappingByIdAsync(model.Id);
        if (mapping == null)
            return Json(new { Result = "error", Errors = new[] { "Mapping was not found." } });

        mapping.AcademicYearGradeSectionMappingId = model.AcademicYearClassSectionMappingId;
        mapping.Amount = model.Amount;

        await _eventService.UpdateAcademicYearGradeSectionEventMappingAsync(mapping);

        return Json(new { Result = "success" });
    }

    [HttpPost]
    [CheckPermission(StandardPermission.HolidaysNEvents.MANAGE_EVENTS)]
    public virtual async Task<IActionResult> AcademicYearGradeSectionEventMappingDelete(int id)
    {
        var mapping = await _eventService.GetAcademicYearGradeSectionEventMappingByIdAsync(id);
        if (mapping == null)
            return new NullJsonResult();

        await _eventService.DeleteAcademicYearGradeSectionEventMappingAsync(mapping);

        return new NullJsonResult();
    }

    #endregion

    #region Events

    public virtual IActionResult Index()
    {
        return RedirectToAction("List");
    }

    [CheckPermission(StandardPermission.HolidaysNEvents.MANAGE_EVENTS)]
    public virtual async Task<IActionResult> List()
    {
        //prepare model
        var model = await _eventModelFactory.PrepareEventSearchModelAsync(new EventSearchModel());

        return View(model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.HolidaysNEvents.MANAGE_EVENTS)]
    public virtual async Task<IActionResult> EventList(EventSearchModel searchModel)
    {
        //prepare model
        var model = await _eventModelFactory.PrepareEventListModelAsync(searchModel);

        return Json(model);
    }

    [CheckPermission(StandardPermission.HolidaysNEvents.MANAGE_EVENTS)]
    public virtual async Task<IActionResult> Create()
    {
        //prepare model
        var model = await _eventModelFactory.PrepareEventModelAsync(new EventModel(), null);

        return View(model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    [CheckPermission(StandardPermission.HolidaysNEvents.MANAGE_EVENTS)]
    public virtual async Task<IActionResult> Create(EventModel model, bool continueEditing)
    {
        if (ModelState.IsValid)
        {
            var @event = model.ToEntity<Event>();
            await _eventService.InsertEventAsync(@event);

            //activity log
            await _customerActivityService.InsertActivityAsync("AddNewEvent",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.AddNewEvent"), @event.Id), @event);

            //locales
            await UpdateLocalesAsync(@event, model);


            //Stores
            await _storeMappingService.SaveStoreMappingsAsync(@event, model.SelectedStoreIds);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.HolidaysNEvents.Events.Added"));

            if (!continueEditing)
                return RedirectToAction("List");

            return RedirectToAction("Edit", new { id = @event.Id });
        }

        //prepare model
        model = await _eventModelFactory.PrepareEventModelAsync(model, null, true);

        //if we got this far, something failed, redisplay form
        return View(model);
    }

    [CheckPermission(StandardPermission.HolidaysNEvents.MANAGE_EVENTS)]
    public virtual async Task<IActionResult> Edit(int id)
    {
        //try to get a event with the specified id
        var @event = await _eventService.GetEventByIdAsync(id);
        if (@event == null)
            return RedirectToAction("List");

        //prepare model
        var model = await _eventModelFactory.PrepareEventModelAsync(null, @event);

        return View(model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    [CheckPermission(StandardPermission.HolidaysNEvents.MANAGE_EVENTS)]
    public virtual async Task<IActionResult> Edit(EventModel model, bool continueEditing)
    {
        //try to get a event with the specified id
        var @event = await _eventService.GetEventByIdAsync(model.Id);
        if (@event == null)
            return RedirectToAction("List");

        if (ModelState.IsValid)
        {
            @event = model.ToEntity(@event);
            await _eventService.UpdateEventAsync(@event);

            //activity log
            await _customerActivityService.InsertActivityAsync("EditEvent",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.EditEvent"), @event.Id), @event);

            //locales
            await UpdateLocalesAsync(@event, model);


            //Stores
            await _storeMappingService.SaveStoreMappingsAsync(@event, model.SelectedStoreIds);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.HolidaysNEvents.Events.Updated"));

            if (!continueEditing)
                return RedirectToAction("List");

            return RedirectToAction("Edit", new { id = @event.Id });
        }

        //prepare model
        model = await _eventModelFactory.PrepareEventModelAsync(model, @event, true);

        //if we got this far, something failed, redisplay form
        return View(model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.HolidaysNEvents.MANAGE_EVENTS)]
    public virtual async Task<IActionResult> Delete(int id)
    {
        //try to get a event with the specified id
        var @event = await _eventService.GetEventByIdAsync(id);
        if (@event == null)
            return RedirectToAction("List");

        try
        {
            await _eventService.DeleteEventAsync(@event);

            //activity log
            await _customerActivityService.InsertActivityAsync("DeleteEvent",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.DeleteEvent"), @event.Id), @event);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.HolidaysNEvents.Events.Deleted"));

            return RedirectToAction("List");
        }
        catch (Exception exc)
        {
            await _notificationService.ErrorNotificationAsync(exc);
            return RedirectToAction("Edit", new { id = @event.Id });
        }
    }

    

    #endregion
}
