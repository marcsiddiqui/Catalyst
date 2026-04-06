using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.GenericDropDowns;
using Nop.Services.GenericDropDowns;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Services.Stores;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.GenericDropDowns;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Web.Areas.Admin.Controllers;

public partial class GenericDropDownOptionController : BaseAdminController
{
    #region Fields

    protected readonly IGenericDropDownOptionModelFactory _genericDropDownOptionModelFactory;
    protected readonly IGenericDropDownOptionService _genericDropDownOptionService;
    protected readonly ICustomerActivityService _customerActivityService;
    protected readonly ILocalizationService _localizationService;
    protected readonly ILocalizedEntityService _localizedEntityService;
    protected readonly INotificationService _notificationService;
    protected readonly IStoreMappingService _storeMappingService;

    #endregion

    #region Ctor

    public GenericDropDownOptionController(
        IGenericDropDownOptionModelFactory genericDropDownOptionModelFactory,
        IGenericDropDownOptionService genericDropDownOptionService,
        ICustomerActivityService customerActivityService,
        ILocalizationService localizationService,
        ILocalizedEntityService localizedEntityService,
        INotificationService notificationService,
        IStoreMappingService storeMappingService)
    {
        _genericDropDownOptionModelFactory = genericDropDownOptionModelFactory;
        _genericDropDownOptionService = genericDropDownOptionService;
        _customerActivityService = customerActivityService;
        _localizationService = localizationService;
        _localizedEntityService = localizedEntityService;
        _notificationService = notificationService;
        _storeMappingService = storeMappingService;
    }

    #endregion

    #region Utilities

    protected virtual async Task UpdateLocalesAsync(GenericDropDownOption genericDropDownOption, GenericDropDownOptionModel model)
    {
        foreach (var localized in model.Locales)
        {
            await _localizedEntityService.SaveLocalizedValueAsync(genericDropDownOption,
                x => x.Text,
                localized.Text,
                localized.LanguageId);


        }
    }

    #endregion

    #region GenericDropDownOptions

    public virtual IActionResult Index()
    {
        return RedirectToAction("List");
    }

    [CheckPermission(StandardPermission.GenericDropDowns.MANAGE_GENERICDROPDOWNOPTIONS)]
    public virtual async Task<IActionResult> List()
    {
        //prepare model
        var model = await _genericDropDownOptionModelFactory.PrepareGenericDropDownOptionSearchModelAsync(new GenericDropDownOptionSearchModel());

        return View(model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.GenericDropDowns.MANAGE_GENERICDROPDOWNOPTIONS)]
    public virtual async Task<IActionResult> GenericDropDownOptionList(GenericDropDownOptionSearchModel searchModel)
    {
        //prepare model
        var model = await _genericDropDownOptionModelFactory.PrepareGenericDropDownOptionListModelAsync(searchModel);

        return Json(model);
    }

    [CheckPermission(StandardPermission.GenericDropDowns.MANAGE_GENERICDROPDOWNOPTIONS)]
    public virtual async Task<IActionResult> Create()
    {
        //prepare model
        var model = await _genericDropDownOptionModelFactory.PrepareGenericDropDownOptionModelAsync(new GenericDropDownOptionModel(), null);

        return View(model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    [CheckPermission(StandardPermission.GenericDropDowns.MANAGE_GENERICDROPDOWNOPTIONS)]
    public virtual async Task<IActionResult> Create(GenericDropDownOptionModel model, bool continueEditing)
    {
        if (ModelState.IsValid)
        {
            var genericDropDownOption = model.ToEntity<GenericDropDownOption>();
            await _genericDropDownOptionService.InsertGenericDropDownOptionAsync(genericDropDownOption);

            //activity log
            await _customerActivityService.InsertActivityAsync("AddNewGenericDropDownOption",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.AddNewGenericDropDownOption"), genericDropDownOption.Id), genericDropDownOption);

            //locales
            await UpdateLocalesAsync(genericDropDownOption, model);


//{{StoreMappingSaveMethodCallHere}}

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.GenericDropDowns.GenericDropDownOptions.Added"));

            if (!continueEditing)
                return RedirectToAction("List");

            return RedirectToAction("Edit", new { id = genericDropDownOption.Id });
        }

        //prepare model
        model = await _genericDropDownOptionModelFactory.PrepareGenericDropDownOptionModelAsync(model, null, true);

        //if we got this far, something failed, redisplay form
        return View(model);
    }

    [CheckPermission(StandardPermission.GenericDropDowns.MANAGE_GENERICDROPDOWNOPTIONS)]
    public virtual async Task<IActionResult> Edit(int id)
    {
        //try to get a genericDropDownOption with the specified id
        var genericDropDownOption = await _genericDropDownOptionService.GetGenericDropDownOptionByIdAsync(id);
        if (genericDropDownOption == null)
            return RedirectToAction("List");

        //prepare model
        var model = await _genericDropDownOptionModelFactory.PrepareGenericDropDownOptionModelAsync(null, genericDropDownOption);

        return View(model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    [CheckPermission(StandardPermission.GenericDropDowns.MANAGE_GENERICDROPDOWNOPTIONS)]
    public virtual async Task<IActionResult> Edit(GenericDropDownOptionModel model, bool continueEditing)
    {
        //try to get a genericDropDownOption with the specified id
        var genericDropDownOption = await _genericDropDownOptionService.GetGenericDropDownOptionByIdAsync(model.Id);
        if (genericDropDownOption == null)
            return RedirectToAction("List");

        if (ModelState.IsValid)
        {
            genericDropDownOption = model.ToEntity(genericDropDownOption);
            await _genericDropDownOptionService.UpdateGenericDropDownOptionAsync(genericDropDownOption);

            //activity log
            await _customerActivityService.InsertActivityAsync("EditGenericDropDownOption",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.EditGenericDropDownOption"), genericDropDownOption.Id), genericDropDownOption);

            //locales
            await UpdateLocalesAsync(genericDropDownOption, model);


//{{StoreMappingSaveMethodCallHere}}

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.GenericDropDowns.GenericDropDownOptions.Updated"));

            if (!continueEditing)
                return RedirectToAction("List");

            return RedirectToAction("Edit", new { id = genericDropDownOption.Id });
        }

        //prepare model
        model = await _genericDropDownOptionModelFactory.PrepareGenericDropDownOptionModelAsync(model, genericDropDownOption, true);

        //if we got this far, something failed, redisplay form
        return View(model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.GenericDropDowns.MANAGE_GENERICDROPDOWNOPTIONS)]
    public virtual async Task<IActionResult> Delete(int id)
    {
        //try to get a genericDropDownOption with the specified id
        var genericDropDownOption = await _genericDropDownOptionService.GetGenericDropDownOptionByIdAsync(id);
        if (genericDropDownOption == null)
            return RedirectToAction("List");

        try
        {
            await _genericDropDownOptionService.DeleteGenericDropDownOptionAsync(genericDropDownOption);

            //activity log
            await _customerActivityService.InsertActivityAsync("DeleteGenericDropDownOption",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.DeleteGenericDropDownOption"), genericDropDownOption.Id), genericDropDownOption);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.GenericDropDowns.GenericDropDownOptions.Deleted"));

            return RedirectToAction("List");
        }
        catch (Exception exc)
        {
            await _notificationService.ErrorNotificationAsync(exc);
            return RedirectToAction("Edit", new { id = genericDropDownOption.Id });
        }
    }

    

    #endregion
}