using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.Admissions;
using Nop.Services.Admissions;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Services.Stores;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.Admissions;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Web.Areas.Admin.Controllers;

public partial class AdmissionController : BaseAdminController
{
    #region Fields

    protected readonly IAdmissionModelFactory _admissionModelFactory;
    protected readonly IAdmissionService _admissionService;
    protected readonly ICustomerActivityService _customerActivityService;
    protected readonly ILocalizationService _localizationService;
    protected readonly ILocalizedEntityService _localizedEntityService;
    protected readonly INotificationService _notificationService;
    protected readonly IStoreMappingService _storeMappingService;

    #endregion

    #region Ctor

    public AdmissionController(
        IAdmissionModelFactory admissionModelFactory,
        IAdmissionService admissionService,
        ICustomerActivityService customerActivityService,
        ILocalizationService localizationService,
        ILocalizedEntityService localizedEntityService,
        INotificationService notificationService,
        IStoreMappingService storeMappingService)
    {
        _admissionModelFactory = admissionModelFactory;
        _admissionService = admissionService;
        _customerActivityService = customerActivityService;
        _localizationService = localizationService;
        _localizedEntityService = localizedEntityService;
        _notificationService = notificationService;
        _storeMappingService = storeMappingService;
    }

    #endregion

    #region Utilities



    #endregion

    #region Admissions

    public virtual IActionResult Index()
    {
        return RedirectToAction("List");
    }

    [CheckPermission(StandardPermission.Admissions.MANAGE_ADMISSIONS)]
    public virtual async Task<IActionResult> List()
    {
        //prepare model
        var model = await _admissionModelFactory.PrepareAdmissionSearchModelAsync(new AdmissionSearchModel());

        return View(model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Admissions.MANAGE_ADMISSIONS)]
    public virtual async Task<IActionResult> AdmissionList(AdmissionSearchModel searchModel)
    {
        //prepare model
        var model = await _admissionModelFactory.PrepareAdmissionListModelAsync(searchModel);

        return Json(model);
    }

    [CheckPermission(StandardPermission.Admissions.MANAGE_ADMISSIONS)]
    public virtual async Task<IActionResult> Create()
    {
        //prepare model
        var model = await _admissionModelFactory.PrepareAdmissionModelAsync(new AdmissionModel(), null);

        return View(model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    [CheckPermission(StandardPermission.Admissions.MANAGE_ADMISSIONS)]
    public virtual async Task<IActionResult> Create(AdmissionModel model, bool continueEditing)
    {
        if (ModelState.IsValid)
        {
            var admission = model.ToEntity<Admission>();
            await _admissionService.InsertAdmissionAsync(admission);

            //activity log
            await _customerActivityService.InsertActivityAsync("AddNewAdmission",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.AddNewAdmission"), admission.Id), admission);



//{{StoreMappingSaveMethodCallHere}}

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Admissions.Admissions.Added"));

            if (!continueEditing)
                return RedirectToAction("List");

            return RedirectToAction("Edit", new { id = admission.Id });
        }

        //prepare model
        model = await _admissionModelFactory.PrepareAdmissionModelAsync(model, null, true);

        //if we got this far, something failed, redisplay form
        return View(model);
    }

    [CheckPermission(StandardPermission.Admissions.MANAGE_ADMISSIONS)]
    public virtual async Task<IActionResult> Edit(int id)
    {
        //try to get a admission with the specified id
        var admission = await _admissionService.GetAdmissionByIdAsync(id);
        if (admission == null)
            return RedirectToAction("List");

        //prepare model
        var model = await _admissionModelFactory.PrepareAdmissionModelAsync(null, admission);

        return View(model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    [CheckPermission(StandardPermission.Admissions.MANAGE_ADMISSIONS)]
    public virtual async Task<IActionResult> Edit(AdmissionModel model, bool continueEditing)
    {
        //try to get a admission with the specified id
        var admission = await _admissionService.GetAdmissionByIdAsync(model.Id);
        if (admission == null)
            return RedirectToAction("List");

        if (ModelState.IsValid)
        {
            admission = model.ToEntity(admission);
            await _admissionService.UpdateAdmissionAsync(admission);

            //activity log
            await _customerActivityService.InsertActivityAsync("EditAdmission",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.EditAdmission"), admission.Id), admission);



//{{StoreMappingSaveMethodCallHere}}

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Admissions.Admissions.Updated"));

            if (!continueEditing)
                return RedirectToAction("List");

            return RedirectToAction("Edit", new { id = admission.Id });
        }

        //prepare model
        model = await _admissionModelFactory.PrepareAdmissionModelAsync(model, admission, true);

        //if we got this far, something failed, redisplay form
        return View(model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Admissions.MANAGE_ADMISSIONS)]
    public virtual async Task<IActionResult> Delete(int id)
    {
        //try to get a admission with the specified id
        var admission = await _admissionService.GetAdmissionByIdAsync(id);
        if (admission == null)
            return RedirectToAction("List");

        try
        {
            await _admissionService.DeleteAdmissionAsync(admission);

            //activity log
            await _customerActivityService.InsertActivityAsync("DeleteAdmission",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.DeleteAdmission"), admission.Id), admission);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Admissions.Admissions.Deleted"));

            return RedirectToAction("List");
        }
        catch (Exception exc)
        {
            await _notificationService.ErrorNotificationAsync(exc);
            return RedirectToAction("Edit", new { id = admission.Id });
        }
    }

    

    #endregion
}