using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.Fees;
using Nop.Services.Fees;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Services.Stores;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.Fees;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Web.Areas.Admin.Controllers;

public partial class FeeController : BaseAdminController
{
    #region Fields

    protected readonly IFeeModelFactory _feeModelFactory;
    protected readonly IFeeService _feeService;
    protected readonly ICustomerActivityService _customerActivityService;
    protected readonly ILocalizationService _localizationService;
    protected readonly ILocalizedEntityService _localizedEntityService;
    protected readonly INotificationService _notificationService;
    protected readonly IStoreMappingService _storeMappingService;

    #endregion

    #region Ctor

    public FeeController(
        IFeeModelFactory feeModelFactory,
        IFeeService feeService,
        ICustomerActivityService customerActivityService,
        ILocalizationService localizationService,
        ILocalizedEntityService localizedEntityService,
        INotificationService notificationService,
        IStoreMappingService storeMappingService)
    {
        _feeModelFactory = feeModelFactory;
        _feeService = feeService;
        _customerActivityService = customerActivityService;
        _localizationService = localizationService;
        _localizedEntityService = localizedEntityService;
        _notificationService = notificationService;
        _storeMappingService = storeMappingService;
    }

    #endregion

    #region Utilities



    #endregion

    #region Fees

    public virtual IActionResult Index()
    {
        return RedirectToAction("List");
    }

    [CheckPermission(StandardPermission.Fees.MANAGE_FEES)]
    public virtual async Task<IActionResult> List()
    {
        //prepare model
        var model = await _feeModelFactory.PrepareFeeSearchModelAsync(new FeeSearchModel());

        return View(model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Fees.MANAGE_FEES)]
    public virtual async Task<IActionResult> FeeList(FeeSearchModel searchModel)
    {
        //prepare model
        var model = await _feeModelFactory.PrepareFeeListModelAsync(searchModel);

        return Json(model);
    }

    [CheckPermission(StandardPermission.Fees.MANAGE_FEES)]
    public virtual async Task<IActionResult> Create()
    {
        //prepare model
        var model = await _feeModelFactory.PrepareFeeModelAsync(new FeeModel(), null);

        return View(model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    [CheckPermission(StandardPermission.Fees.MANAGE_FEES)]
    public virtual async Task<IActionResult> Create(FeeModel model, bool continueEditing)
    {
        if (ModelState.IsValid)
        {
            var fee = model.ToEntity<Fee>();
            await _feeService.InsertFeeAsync(fee);

            //activity log
            await _customerActivityService.InsertActivityAsync("AddNewFee",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.AddNewFee"), fee.Id), fee);



//{{StoreMappingSaveMethodCallHere}}

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Fees.Fees.Added"));

            if (!continueEditing)
                return RedirectToAction("List");

            return RedirectToAction("Edit", new { id = fee.Id });
        }

        //prepare model
        model = await _feeModelFactory.PrepareFeeModelAsync(model, null, true);

        //if we got this far, something failed, redisplay form
        return View(model);
    }

    [CheckPermission(StandardPermission.Fees.MANAGE_FEES)]
    public virtual async Task<IActionResult> Edit(int id)
    {
        //try to get a fee with the specified id
        var fee = await _feeService.GetFeeByIdAsync(id);
        if (fee == null)
            return RedirectToAction("List");

        //prepare model
        var model = await _feeModelFactory.PrepareFeeModelAsync(null, fee);

        return View(model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    [CheckPermission(StandardPermission.Fees.MANAGE_FEES)]
    public virtual async Task<IActionResult> Edit(FeeModel model, bool continueEditing)
    {
        //try to get a fee with the specified id
        var fee = await _feeService.GetFeeByIdAsync(model.Id);
        if (fee == null)
            return RedirectToAction("List");

        if (ModelState.IsValid)
        {
            fee = model.ToEntity(fee);
            await _feeService.UpdateFeeAsync(fee);

            //activity log
            await _customerActivityService.InsertActivityAsync("EditFee",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.EditFee"), fee.Id), fee);



//{{StoreMappingSaveMethodCallHere}}

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Fees.Fees.Updated"));

            if (!continueEditing)
                return RedirectToAction("List");

            return RedirectToAction("Edit", new { id = fee.Id });
        }

        //prepare model
        model = await _feeModelFactory.PrepareFeeModelAsync(model, fee, true);

        //if we got this far, something failed, redisplay form
        return View(model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Fees.MANAGE_FEES)]
    public virtual async Task<IActionResult> Delete(int id)
    {
        //try to get a fee with the specified id
        var fee = await _feeService.GetFeeByIdAsync(id);
        if (fee == null)
            return RedirectToAction("List");

        try
        {
            await _feeService.DeleteFeeAsync(fee);

            //activity log
            await _customerActivityService.InsertActivityAsync("DeleteFee",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.DeleteFee"), fee.Id), fee);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Fees.Fees.Deleted"));

            return RedirectToAction("List");
        }
        catch (Exception exc)
        {
            await _notificationService.ErrorNotificationAsync(exc);
            return RedirectToAction("Edit", new { id = fee.Id });
        }
    }

    

    #endregion
}