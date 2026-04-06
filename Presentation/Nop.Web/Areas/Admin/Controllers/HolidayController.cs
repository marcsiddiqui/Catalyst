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
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Web.Areas.Admin.Controllers;

public partial class HolidayController : BaseAdminController
{
    #region Fields

    protected readonly IHolidayModelFactory _holidayModelFactory;
    protected readonly IHolidayService _holidayService;
    protected readonly ICustomerActivityService _customerActivityService;
    protected readonly ILocalizationService _localizationService;
    protected readonly ILocalizedEntityService _localizedEntityService;
    protected readonly INotificationService _notificationService;
    protected readonly IStoreMappingService _storeMappingService;

    #endregion

    #region Ctor

    public HolidayController(
        IHolidayModelFactory holidayModelFactory,
        IHolidayService holidayService,
        ICustomerActivityService customerActivityService,
        ILocalizationService localizationService,
        ILocalizedEntityService localizedEntityService,
        INotificationService notificationService,
        IStoreMappingService storeMappingService)
    {
        _holidayModelFactory = holidayModelFactory;
        _holidayService = holidayService;
        _customerActivityService = customerActivityService;
        _localizationService = localizationService;
        _localizedEntityService = localizedEntityService;
        _notificationService = notificationService;
        _storeMappingService = storeMappingService;
    }

    #endregion

    #region Utilities

    protected virtual async Task UpdateLocalesAsync(Holiday holiday, HolidayModel model)
    {
        foreach (var localized in model.Locales)
        {
            await _localizedEntityService.SaveLocalizedValueAsync(holiday,
                x => x.Name,
                localized.Name,
                localized.LanguageId);


        }
    }

    #endregion

    #region Holidays

    public virtual IActionResult Index()
    {
        return RedirectToAction("List");
    }

    [CheckPermission(StandardPermission.HolidaysNEvents.MANAGE_HOLIDAYS)]
    public virtual async Task<IActionResult> List()
    {
        //prepare model
        var model = await _holidayModelFactory.PrepareHolidaySearchModelAsync(new HolidaySearchModel());

        return View(model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.HolidaysNEvents.MANAGE_HOLIDAYS)]
    public virtual async Task<IActionResult> HolidayList(HolidaySearchModel searchModel)
    {
        //prepare model
        var model = await _holidayModelFactory.PrepareHolidayListModelAsync(searchModel);

        return Json(model);
    }

    [CheckPermission(StandardPermission.HolidaysNEvents.MANAGE_HOLIDAYS)]
    public virtual async Task<IActionResult> Create()
    {
        //prepare model
        var model = await _holidayModelFactory.PrepareHolidayModelAsync(new HolidayModel(), null);

        return View(model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    [CheckPermission(StandardPermission.HolidaysNEvents.MANAGE_HOLIDAYS)]
    public virtual async Task<IActionResult> Create(HolidayModel model, bool continueEditing)
    {
        if (ModelState.IsValid)
        {
            var holiday = model.ToEntity<Holiday>();
            await _holidayService.InsertHolidayAsync(holiday);

            //activity log
            await _customerActivityService.InsertActivityAsync("AddNewHoliday",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.AddNewHoliday"), holiday.Id), holiday);

            //locales
            await UpdateLocalesAsync(holiday, model);


            //Stores            await _storeMappingService.SaveStoreMappingsAsync(holiday, model.SelectedStoreIds);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.HolidaysNEvents.Holidays.Added"));

            if (!continueEditing)
                return RedirectToAction("List");

            return RedirectToAction("Edit", new { id = holiday.Id });
        }

        //prepare model
        model = await _holidayModelFactory.PrepareHolidayModelAsync(model, null, true);

        //if we got this far, something failed, redisplay form
        return View(model);
    }

    [CheckPermission(StandardPermission.HolidaysNEvents.MANAGE_HOLIDAYS)]
    public virtual async Task<IActionResult> Edit(int id)
    {
        //try to get a holiday with the specified id
        var holiday = await _holidayService.GetHolidayByIdAsync(id);
        if (holiday == null)
            return RedirectToAction("List");

        //prepare model
        var model = await _holidayModelFactory.PrepareHolidayModelAsync(null, holiday);

        return View(model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    [CheckPermission(StandardPermission.HolidaysNEvents.MANAGE_HOLIDAYS)]
    public virtual async Task<IActionResult> Edit(HolidayModel model, bool continueEditing)
    {
        //try to get a holiday with the specified id
        var holiday = await _holidayService.GetHolidayByIdAsync(model.Id);
        if (holiday == null)
            return RedirectToAction("List");

        if (ModelState.IsValid)
        {
            holiday = model.ToEntity(holiday);
            await _holidayService.UpdateHolidayAsync(holiday);

            //activity log
            await _customerActivityService.InsertActivityAsync("EditHoliday",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.EditHoliday"), holiday.Id), holiday);

            //locales
            await UpdateLocalesAsync(holiday, model);


            //Stores            await _storeMappingService.SaveStoreMappingsAsync(holiday, model.SelectedStoreIds);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.HolidaysNEvents.Holidays.Updated"));

            if (!continueEditing)
                return RedirectToAction("List");

            return RedirectToAction("Edit", new { id = holiday.Id });
        }

        //prepare model
        model = await _holidayModelFactory.PrepareHolidayModelAsync(model, holiday, true);

        //if we got this far, something failed, redisplay form
        return View(model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.HolidaysNEvents.MANAGE_HOLIDAYS)]
    public virtual async Task<IActionResult> Delete(int id)
    {
        //try to get a holiday with the specified id
        var holiday = await _holidayService.GetHolidayByIdAsync(id);
        if (holiday == null)
            return RedirectToAction("List");

        try
        {
            await _holidayService.DeleteHolidayAsync(holiday);

            //activity log
            await _customerActivityService.InsertActivityAsync("DeleteHoliday",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.DeleteHoliday"), holiday.Id), holiday);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.HolidaysNEvents.Holidays.Deleted"));

            return RedirectToAction("List");
        }
        catch (Exception exc)
        {
            await _notificationService.ErrorNotificationAsync(exc);
            return RedirectToAction("Edit", new { id = holiday.Id });
        }
    }

    

    #endregion
}