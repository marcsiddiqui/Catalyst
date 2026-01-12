using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.AcademicYears;
using Nop.Services.AcademicYears;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Services.Stores;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.AcademicYears;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Web.Areas.Admin.Controllers;

public partial class AcademicYearController : BaseAdminController
{
    #region Fields

    protected readonly IAcademicYearModelFactory _academicYearModelFactory;
    protected readonly IAcademicYearService _academicYearService;
    protected readonly ICustomerActivityService _customerActivityService;
    protected readonly ILocalizationService _localizationService;
    protected readonly ILocalizedEntityService _localizedEntityService;
    protected readonly INotificationService _notificationService;
    protected readonly IStoreMappingService _storeMappingService;
    protected readonly IAcademicYearTermModelFactory _academicYearTermModelFactory;

    #endregion

    #region Ctor

    public AcademicYearController(
        IAcademicYearModelFactory academicYearModelFactory,
        IAcademicYearService academicYearService,
        ICustomerActivityService customerActivityService,
        ILocalizationService localizationService,
        ILocalizedEntityService localizedEntityService,
        INotificationService notificationService,
        IStoreMappingService storeMappingService,
        IAcademicYearTermModelFactory academicYearTermModelFactory
        )
    {
        _academicYearModelFactory = academicYearModelFactory;
        _academicYearService = academicYearService;
        _customerActivityService = customerActivityService;
        _localizationService = localizationService;
        _localizedEntityService = localizedEntityService;
        _notificationService = notificationService;
        _storeMappingService = storeMappingService;
        _academicYearTermModelFactory = academicYearTermModelFactory;
    }

    #endregion

    #region Utilities

    protected virtual async Task UpdateLocalesAsync(AcademicYear academicYear, AcademicYearModel model)
    {
        foreach (var localized in model.Locales)
        {
            await _localizedEntityService.SaveLocalizedValueAsync(academicYear,
                x => x.Name,
                localized.Name,
                localized.LanguageId);


        }
    }

    protected virtual async Task AcademicYearTermUpdateLocalesAsync(AcademicYearTerm academicYearTerm, AcademicYearTermModel model)
    {
        foreach (var localized in model.Locales)
        {
            await _localizedEntityService.SaveLocalizedValueAsync(academicYearTerm,
                x => x.Name,
                localized.Name,
                localized.LanguageId);


        }
    }

    #endregion

    #region AcademicYears

    public virtual IActionResult Index()
    {
        return RedirectToAction("List");
    }

    [CheckPermission(StandardPermission.AcademicYears.MANAGE_ACADEMICYEARS)]
    public virtual async Task<IActionResult> List()
    {
        //prepare model
        var model = await _academicYearModelFactory.PrepareAcademicYearSearchModelAsync(new AcademicYearSearchModel());

        return View(model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.AcademicYears.MANAGE_ACADEMICYEARS)]
    public virtual async Task<IActionResult> AcademicYearList(AcademicYearSearchModel searchModel)
    {
        //prepare model
        var model = await _academicYearModelFactory.PrepareAcademicYearListModelAsync(searchModel);

        return Json(model);
    }

    [CheckPermission(StandardPermission.AcademicYears.MANAGE_ACADEMICYEARS)]
    public virtual async Task<IActionResult> Create()
    {
        //prepare model
        var model = await _academicYearModelFactory.PrepareAcademicYearModelAsync(new AcademicYearModel(), null);

        return View(model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    [CheckPermission(StandardPermission.AcademicYears.MANAGE_ACADEMICYEARS)]
    public virtual async Task<IActionResult> Create(AcademicYearModel model, bool continueEditing)
    {
        if (ModelState.IsValid)
        {
            var academicYear = model.ToEntity<AcademicYear>();
            await _academicYearService.InsertAcademicYearAsync(academicYear);

            //activity log
            await _customerActivityService.InsertActivityAsync("AddNewAcademicYear",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.AddNewAcademicYear"), academicYear.Id), academicYear);

            //locales
            await UpdateLocalesAsync(academicYear, model);


//{{StoreMappingSaveMethodCallHere}}

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.AcademicYears.AcademicYears.Added"));

            if (!continueEditing)
                return RedirectToAction("List");

            return RedirectToAction("Edit", new { id = academicYear.Id });
        }

        //prepare model
        model = await _academicYearModelFactory.PrepareAcademicYearModelAsync(model, null, true);

        //if we got this far, something failed, redisplay form
        return View(model);
    }

    [CheckPermission(StandardPermission.AcademicYears.MANAGE_ACADEMICYEARS)]
    public virtual async Task<IActionResult> Edit(int id)
    {
        //try to get a academicYear with the specified id
        var academicYear = await _academicYearService.GetAcademicYearByIdAsync(id);
        if (academicYear == null)
            return RedirectToAction("List");

        //prepare model
        var model = await _academicYearModelFactory.PrepareAcademicYearModelAsync(null, academicYear);

        return View(model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    [CheckPermission(StandardPermission.AcademicYears.MANAGE_ACADEMICYEARS)]
    public virtual async Task<IActionResult> Edit(AcademicYearModel model, bool continueEditing)
    {
        //try to get a academicYear with the specified id
        var academicYear = await _academicYearService.GetAcademicYearByIdAsync(model.Id);
        if (academicYear == null)
            return RedirectToAction("List");

        if (ModelState.IsValid)
        {
            academicYear = model.ToEntity(academicYear);
            await _academicYearService.UpdateAcademicYearAsync(academicYear);

            //activity log
            await _customerActivityService.InsertActivityAsync("EditAcademicYear",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.EditAcademicYear"), academicYear.Id), academicYear);

            //locales
            await UpdateLocalesAsync(academicYear, model);


//{{StoreMappingSaveMethodCallHere}}

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.AcademicYears.AcademicYears.Updated"));

            if (!continueEditing)
                return RedirectToAction("List");

            return RedirectToAction("Edit", new { id = academicYear.Id });
        }

        //prepare model
        model = await _academicYearModelFactory.PrepareAcademicYearModelAsync(model, academicYear, true);

        //if we got this far, something failed, redisplay form
        return View(model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.AcademicYears.MANAGE_ACADEMICYEARS)]
    public virtual async Task<IActionResult> Delete(int id)
    {
        //try to get a academicYear with the specified id
        var academicYear = await _academicYearService.GetAcademicYearByIdAsync(id);
        if (academicYear == null)
            return RedirectToAction("List");

        try
        {
            await _academicYearService.DeleteAcademicYearAsync(academicYear);

            //activity log
            await _customerActivityService.InsertActivityAsync("DeleteAcademicYear",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.DeleteAcademicYear"), academicYear.Id), academicYear);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.AcademicYears.AcademicYears.Deleted"));

            return RedirectToAction("List");
        }
        catch (Exception exc)
        {
            await _notificationService.ErrorNotificationAsync(exc);
            return RedirectToAction("Edit", new { id = academicYear.Id });
        }
    }



    #endregion

    #region AcademicYearTerms

    [CheckPermission(StandardPermission.AcademicYears.MANAGE_ACADEMICYEARTERMS)]
    public virtual async Task<IActionResult> AcademicYearTermList()
    {
        //prepare model
        var model = await _academicYearTermModelFactory.PrepareAcademicYearTermSearchModelAsync(new AcademicYearTermSearchModel());

        return View(model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.AcademicYears.MANAGE_ACADEMICYEARTERMS)]
    public virtual async Task<IActionResult> AcademicYearTermGridList(AcademicYearTermSearchModel searchModel)
    {
        //prepare model
        var model = await _academicYearTermModelFactory.PrepareAcademicYearTermListModelAsync(searchModel);

        return Json(model);
    }

    [CheckPermission(StandardPermission.AcademicYears.MANAGE_ACADEMICYEARTERMS)]
    public virtual async Task<IActionResult> AcademicYearTermCreate()
    {
        //prepare model
        var model = await _academicYearTermModelFactory.PrepareAcademicYearTermModelAsync(new AcademicYearTermModel(), null);

        return View(model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    [CheckPermission(StandardPermission.AcademicYears.MANAGE_ACADEMICYEARTERMS)]
    public virtual async Task<IActionResult> AcademicYearTermCreate(AcademicYearTermModel model, bool continueEditing)
    {
        if (ModelState.IsValid)
        {
            var academicYearTerm = model.ToEntity<AcademicYearTerm>();
            await _academicYearService.InsertAcademicYearTermAsync(academicYearTerm);

            //activity log
            await _customerActivityService.InsertActivityAsync("AddNewAcademicYearTerm",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.AddNewAcademicYearTerm"), academicYearTerm.Id), academicYearTerm);

            //locales
            await AcademicYearTermUpdateLocalesAsync(academicYearTerm, model);


            //{{StoreMappingSaveMethodCallHere}}

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Academics.AcademicYearTerms.Added"));

            if (!continueEditing)
                return RedirectToAction("List");

            return RedirectToAction("Edit", new { id = academicYearTerm.Id });
        }

        //prepare model
        model = await _academicYearTermModelFactory.PrepareAcademicYearTermModelAsync(model, null, true);

        //if we got this far, something failed, redisplay form
        return View(model);
    }

    [CheckPermission(StandardPermission.AcademicYears.MANAGE_ACADEMICYEARTERMS)]
    public virtual async Task<IActionResult> AcademicYearTermEdit(int id)
    {
        //try to get a academicYearTerm with the specified id
        var academicYearTerm = await _academicYearService.GetAcademicYearTermByIdAsync(id);
        if (academicYearTerm == null)
            return RedirectToAction("List");

        //prepare model
        var model = await _academicYearTermModelFactory.PrepareAcademicYearTermModelAsync(null, academicYearTerm);

        return View(model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    [CheckPermission(StandardPermission.AcademicYears.MANAGE_ACADEMICYEARTERMS)]
    public virtual async Task<IActionResult> AcademicYearTermEdit(AcademicYearTermModel model, bool continueEditing)
    {
        //try to get a academicYearTerm with the specified id
        var academicYearTerm = await _academicYearService.GetAcademicYearTermByIdAsync(model.Id);
        if (academicYearTerm == null)
            return RedirectToAction("List");

        if (ModelState.IsValid)
        {
            academicYearTerm = model.ToEntity(academicYearTerm);
            await _academicYearService.UpdateAcademicYearTermAsync(academicYearTerm);

            //activity log
            await _customerActivityService.InsertActivityAsync("EditAcademicYearTerm",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.EditAcademicYearTerm"), academicYearTerm.Id), academicYearTerm);

            //locales
            await AcademicYearTermUpdateLocalesAsync(academicYearTerm, model);


            //{{StoreMappingSaveMethodCallHere}}

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Academics.AcademicYearTerms.Updated"));

            if (!continueEditing)
                return RedirectToAction("List");

            return RedirectToAction("Edit", new { id = academicYearTerm.Id });
        }

        //prepare model
        model = await _academicYearTermModelFactory.PrepareAcademicYearTermModelAsync(model, academicYearTerm, true);

        //if we got this far, something failed, redisplay form
        return View(model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.AcademicYears.MANAGE_ACADEMICYEARTERMS)]
    public virtual async Task<IActionResult> AcademicYearTermDelete(int id)
    {
        //try to get a academicYearTerm with the specified id
        var academicYearTerm = await _academicYearService.GetAcademicYearTermByIdAsync(id);
        if (academicYearTerm == null)
            return RedirectToAction("List");

        try
        {
            await _academicYearService.DeleteAcademicYearTermAsync(academicYearTerm);

            //activity log
            await _customerActivityService.InsertActivityAsync("DeleteAcademicYearTerm",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.DeleteAcademicYearTerm"), academicYearTerm.Id), academicYearTerm);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Academics.AcademicYearTerms.Deleted"));

            return RedirectToAction("List");
        }
        catch (Exception exc)
        {
            await _notificationService.ErrorNotificationAsync(exc);
            return RedirectToAction("Edit", new { id = academicYearTerm.Id });
        }
    }

    #endregion
}