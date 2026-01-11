using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.GradeManagement;
using Nop.Services.GradeManagement;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Services.Stores;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.GradeManagement;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Web.Areas.Admin.Controllers;

public partial class GradeController : BaseAdminController
{
    #region Fields

    protected readonly IGradeModelFactory _gradeModelFactory;
    protected readonly IGradeService _gradeService;
    protected readonly ICustomerActivityService _customerActivityService;
    protected readonly ILocalizationService _localizationService;
    protected readonly ILocalizedEntityService _localizedEntityService;
    protected readonly INotificationService _notificationService;
    protected readonly IStoreMappingService _storeMappingService;

    #endregion

    #region Ctor

    public GradeController(
        IGradeModelFactory gradeModelFactory,
        IGradeService gradeService,
        ICustomerActivityService customerActivityService,
        ILocalizationService localizationService,
        ILocalizedEntityService localizedEntityService,
        INotificationService notificationService,
        IStoreMappingService storeMappingService)
    {
        _gradeModelFactory = gradeModelFactory;
        _gradeService = gradeService;
        _customerActivityService = customerActivityService;
        _localizationService = localizationService;
        _localizedEntityService = localizedEntityService;
        _notificationService = notificationService;
        _storeMappingService = storeMappingService;
    }

    #endregion

    #region Utilities

    protected virtual async Task UpdateLocalesAsync(Grade grade, GradeModel model)
    {
        foreach (var localized in model.Locales)
        {
            await _localizedEntityService.SaveLocalizedValueAsync(grade,
                x => x.Name,
                localized.Name,
                localized.LanguageId);


        }
    }

    #endregion

    #region Grades

    public virtual IActionResult Index()
    {
        return RedirectToAction("List");
    }

    [CheckPermission(StandardPermission.GradeManagement.MANAGE_GRADES)]
    public virtual async Task<IActionResult> List()
    {
        //prepare model
        var model = await _gradeModelFactory.PrepareGradeSearchModelAsync(new GradeSearchModel());

        return View(model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.GradeManagement.MANAGE_GRADES)]
    public virtual async Task<IActionResult> GradeList(GradeSearchModel searchModel)
    {
        //prepare model
        var model = await _gradeModelFactory.PrepareGradeListModelAsync(searchModel);

        return Json(model);
    }

    [CheckPermission(StandardPermission.GradeManagement.MANAGE_GRADES)]
    public virtual async Task<IActionResult> Create()
    {
        //prepare model
        var model = await _gradeModelFactory.PrepareGradeModelAsync(new GradeModel(), null);

        return View(model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    [CheckPermission(StandardPermission.GradeManagement.MANAGE_GRADES)]
    public virtual async Task<IActionResult> Create(GradeModel model, bool continueEditing)
    {
        if (ModelState.IsValid)
        {
            var grade = model.ToEntity<Grade>();
            await _gradeService.InsertGradeAsync(grade);

            //activity log
            await _customerActivityService.InsertActivityAsync("AddNewGrade",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.AddNewGrade"), grade.Id), grade);

            //locales
            await UpdateLocalesAsync(grade, model);


            //Stores            await _storeMappingService.SaveStoreMappingsAsync(grade, model.SelectedStoreIds);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.GradeManagement.Grades.Added"));

            if (!continueEditing)
                return RedirectToAction("List");

            return RedirectToAction("Edit", new { id = grade.Id });
        }

        //prepare model
        model = await _gradeModelFactory.PrepareGradeModelAsync(model, null, true);

        //if we got this far, something failed, redisplay form
        return View(model);
    }

    [CheckPermission(StandardPermission.GradeManagement.MANAGE_GRADES)]
    public virtual async Task<IActionResult> Edit(int id)
    {
        //try to get a grade with the specified id
        var grade = await _gradeService.GetGradeByIdAsync(id);
        if (grade == null)
            return RedirectToAction("List");

        //prepare model
        var model = await _gradeModelFactory.PrepareGradeModelAsync(null, grade);

        return View(model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    [CheckPermission(StandardPermission.GradeManagement.MANAGE_GRADES)]
    public virtual async Task<IActionResult> Edit(GradeModel model, bool continueEditing)
    {
        //try to get a grade with the specified id
        var grade = await _gradeService.GetGradeByIdAsync(model.Id);
        if (grade == null)
            return RedirectToAction("List");

        if (ModelState.IsValid)
        {
            grade = model.ToEntity(grade);
            await _gradeService.UpdateGradeAsync(grade);

            //activity log
            await _customerActivityService.InsertActivityAsync("EditGrade",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.EditGrade"), grade.Id), grade);

            //locales
            await UpdateLocalesAsync(grade, model);


            //Stores            await _storeMappingService.SaveStoreMappingsAsync(grade, model.SelectedStoreIds);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.GradeManagement.Grades.Updated"));

            if (!continueEditing)
                return RedirectToAction("List");

            return RedirectToAction("Edit", new { id = grade.Id });
        }

        //prepare model
        model = await _gradeModelFactory.PrepareGradeModelAsync(model, grade, true);

        //if we got this far, something failed, redisplay form
        return View(model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.GradeManagement.MANAGE_GRADES)]
    public virtual async Task<IActionResult> Delete(int id)
    {
        //try to get a grade with the specified id
        var grade = await _gradeService.GetGradeByIdAsync(id);
        if (grade == null)
            return RedirectToAction("List");

        try
        {
            await _gradeService.DeleteGradeAsync(grade);

            //activity log
            await _customerActivityService.InsertActivityAsync("DeleteGrade",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.DeleteGrade"), grade.Id), grade);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.GradeManagement.Grades.Deleted"));

            return RedirectToAction("List");
        }
        catch (Exception exc)
        {
            await _notificationService.ErrorNotificationAsync(exc);
            return RedirectToAction("Edit", new { id = grade.Id });
        }
    }

        [HttpPost]
    [CheckPermission(StandardPermission.GradeManagement.MANAGE_GRADES)]
    public virtual async Task<IActionResult> ActivateSelected(ICollection<int> selectedIds)
    {
        if (selectedIds == null || !selectedIds.Any())
            return NoContent();

        var grades = await _gradeService.GetGradesByIdsAsync([.. selectedIds]);
        foreach (var grade in grades)
        {
            grade.IsActive = true;
            await _gradeService.UpdateGradeAsync(grade);
        }

        return Json(new { Result = true });
    }

    [HttpPost]
    [CheckPermission(StandardPermission.GradeManagement.MANAGE_GRADES)]
    public virtual async Task<IActionResult> DeactivateSelected(ICollection<int> selectedIds)
    {
        if (selectedIds == null || !selectedIds.Any())
            return NoContent();

        var grades = await _gradeService.GetGradesByIdsAsync(selectedIds.ToArray());
        foreach (var grade in grades)
        {
            grade.IsActive = false;
            await _gradeService.UpdateGradeAsync(grade);
        }

        return Json(new { Result = true });
    }

    #endregion
}