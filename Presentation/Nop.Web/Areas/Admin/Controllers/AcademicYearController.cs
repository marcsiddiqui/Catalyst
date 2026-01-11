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
    protected readonly IAcadamicYearTermModelFactory _acadamicYearTermModelFactory;

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
        IAcadamicYearTermModelFactory acadamicYearTermModelFactory
        )
    {
        _academicYearModelFactory = academicYearModelFactory;
        _academicYearService = academicYearService;
        _customerActivityService = customerActivityService;
        _localizationService = localizationService;
        _localizedEntityService = localizedEntityService;
        _notificationService = notificationService;
        _storeMappingService = storeMappingService;
        _acadamicYearTermModelFactory = acadamicYearTermModelFactory;
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

    protected virtual async Task AcadamicYearTermUpdateLocalesAsync(AcadamicYearTerm acadamicYearTerm, AcadamicYearTermModel model)
    {
        foreach (var localized in model.Locales)
        {
            await _localizedEntityService.SaveLocalizedValueAsync(acadamicYearTerm,
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

    #region AcadamicYearTerms

    [CheckPermission(StandardPermission.AcademicYears.MANAGE_ACADAMICYEARTERMS)]
    public virtual async Task<IActionResult> AcadamicYearTermList()
    {
        //prepare model
        var model = await _acadamicYearTermModelFactory.PrepareAcadamicYearTermSearchModelAsync(new AcadamicYearTermSearchModel());

        return View(model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.AcademicYears.MANAGE_ACADAMICYEARTERMS)]
    public virtual async Task<IActionResult> AcadamicYearTermGridList(AcadamicYearTermSearchModel searchModel)
    {
        //prepare model
        var model = await _acadamicYearTermModelFactory.PrepareAcadamicYearTermListModelAsync(searchModel);

        return Json(model);
    }

    [CheckPermission(StandardPermission.AcademicYears.MANAGE_ACADAMICYEARTERMS)]
    public virtual async Task<IActionResult> AcadamicYearTermCreate()
    {
        //prepare model
        var model = await _acadamicYearTermModelFactory.PrepareAcadamicYearTermModelAsync(new AcadamicYearTermModel(), null);

        return View(model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    [CheckPermission(StandardPermission.AcademicYears.MANAGE_ACADAMICYEARTERMS)]
    public virtual async Task<IActionResult> AcadamicYearTermCreate(AcadamicYearTermModel model, bool continueEditing)
    {
        if (ModelState.IsValid)
        {
            var acadamicYearTerm = model.ToEntity<AcadamicYearTerm>();
            await _academicYearService.InsertAcadamicYearTermAsync(acadamicYearTerm);

            //activity log
            await _customerActivityService.InsertActivityAsync("AddNewAcadamicYearTerm",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.AddNewAcadamicYearTerm"), acadamicYearTerm.Id), acadamicYearTerm);

            //locales
            await AcadamicYearTermUpdateLocalesAsync(acadamicYearTerm, model);


            //{{StoreMappingSaveMethodCallHere}}

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Acadamics.AcadamicYearTerms.Added"));

            if (!continueEditing)
                return RedirectToAction("List");

            return RedirectToAction("Edit", new { id = acadamicYearTerm.Id });
        }

        //prepare model
        model = await _acadamicYearTermModelFactory.PrepareAcadamicYearTermModelAsync(model, null, true);

        //if we got this far, something failed, redisplay form
        return View(model);
    }

    [CheckPermission(StandardPermission.AcademicYears.MANAGE_ACADAMICYEARTERMS)]
    public virtual async Task<IActionResult> AcadamicYearTermEdit(int id)
    {
        //try to get a acadamicYearTerm with the specified id
        var acadamicYearTerm = await _academicYearService.GetAcadamicYearTermByIdAsync(id);
        if (acadamicYearTerm == null)
            return RedirectToAction("List");

        //prepare model
        var model = await _acadamicYearTermModelFactory.PrepareAcadamicYearTermModelAsync(null, acadamicYearTerm);

        return View(model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    [CheckPermission(StandardPermission.AcademicYears.MANAGE_ACADAMICYEARTERMS)]
    public virtual async Task<IActionResult> AcadamicYearTermEdit(AcadamicYearTermModel model, bool continueEditing)
    {
        //try to get a acadamicYearTerm with the specified id
        var acadamicYearTerm = await _academicYearService.GetAcadamicYearTermByIdAsync(model.Id);
        if (acadamicYearTerm == null)
            return RedirectToAction("List");

        if (ModelState.IsValid)
        {
            acadamicYearTerm = model.ToEntity(acadamicYearTerm);
            await _academicYearService.UpdateAcadamicYearTermAsync(acadamicYearTerm);

            //activity log
            await _customerActivityService.InsertActivityAsync("EditAcadamicYearTerm",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.EditAcadamicYearTerm"), acadamicYearTerm.Id), acadamicYearTerm);

            //locales
            await AcadamicYearTermUpdateLocalesAsync(acadamicYearTerm, model);


            //{{StoreMappingSaveMethodCallHere}}

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Acadamics.AcadamicYearTerms.Updated"));

            if (!continueEditing)
                return RedirectToAction("List");

            return RedirectToAction("Edit", new { id = acadamicYearTerm.Id });
        }

        //prepare model
        model = await _acadamicYearTermModelFactory.PrepareAcadamicYearTermModelAsync(model, acadamicYearTerm, true);

        //if we got this far, something failed, redisplay form
        return View(model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.AcademicYears.MANAGE_ACADAMICYEARTERMS)]
    public virtual async Task<IActionResult> AcadamicYearTermDelete(int id)
    {
        //try to get a acadamicYearTerm with the specified id
        var acadamicYearTerm = await _academicYearService.GetAcadamicYearTermByIdAsync(id);
        if (acadamicYearTerm == null)
            return RedirectToAction("List");

        try
        {
            await _academicYearService.DeleteAcadamicYearTermAsync(acadamicYearTerm);

            //activity log
            await _customerActivityService.InsertActivityAsync("DeleteAcadamicYearTerm",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.DeleteAcadamicYearTerm"), acadamicYearTerm.Id), acadamicYearTerm);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Acadamics.AcadamicYearTerms.Deleted"));

            return RedirectToAction("List");
        }
        catch (Exception exc)
        {
            await _notificationService.ErrorNotificationAsync(exc);
            return RedirectToAction("Edit", new { id = acadamicYearTerm.Id });
        }
    }

    #endregion
}