using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.Subjects;
using Nop.Services.Subjects;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Services.Stores;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.Subjects;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Web.Areas.Admin.Controllers;

public partial class SubjectController : BaseAdminController
{
    #region Fields

    protected readonly ISubjectModelFactory _subjectModelFactory;
    protected readonly ISubjectService _subjectService;
    protected readonly ICustomerActivityService _customerActivityService;
    protected readonly ILocalizationService _localizationService;
    protected readonly ILocalizedEntityService _localizedEntityService;
    protected readonly INotificationService _notificationService;
    protected readonly IStoreMappingService _storeMappingService;

    #endregion

    #region Ctor

    public SubjectController(
        ISubjectModelFactory subjectModelFactory,
        ISubjectService subjectService,
        ICustomerActivityService customerActivityService,
        ILocalizationService localizationService,
        ILocalizedEntityService localizedEntityService,
        INotificationService notificationService,
        IStoreMappingService storeMappingService)
    {
        _subjectModelFactory = subjectModelFactory;
        _subjectService = subjectService;
        _customerActivityService = customerActivityService;
        _localizationService = localizationService;
        _localizedEntityService = localizedEntityService;
        _notificationService = notificationService;
        _storeMappingService = storeMappingService;
    }

    #endregion

    #region Utilities

    protected virtual async Task UpdateLocalesAsync(Subject subject, SubjectModel model)
    {
        foreach (var localized in model.Locales)
        {
            await _localizedEntityService.SaveLocalizedValueAsync(subject,
                x => x.Name,
                localized.Name,
                localized.LanguageId);


        }
    }

    #endregion

    #region Subjects

    public virtual IActionResult Index()
    {
        return RedirectToAction("List");
    }

    [CheckPermission(StandardPermission.Subjects.MANAGE_SUBJECTS)]
    public virtual async Task<IActionResult> List()
    {
        //prepare model
        var model = await _subjectModelFactory.PrepareSubjectSearchModelAsync(new SubjectSearchModel());

        return View(model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Subjects.MANAGE_SUBJECTS)]
    public virtual async Task<IActionResult> SubjectList(SubjectSearchModel searchModel)
    {
        //prepare model
        var model = await _subjectModelFactory.PrepareSubjectListModelAsync(searchModel);

        return Json(model);
    }

    [CheckPermission(StandardPermission.Subjects.MANAGE_SUBJECTS)]
    public virtual async Task<IActionResult> Create()
    {
        //prepare model
        var model = await _subjectModelFactory.PrepareSubjectModelAsync(new SubjectModel(), null);

        return View(model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    [CheckPermission(StandardPermission.Subjects.MANAGE_SUBJECTS)]
    public virtual async Task<IActionResult> Create(SubjectModel model, bool continueEditing)
    {
        if (ModelState.IsValid)
        {
            var subject = model.ToEntity<Subject>();
            await _subjectService.InsertSubjectAsync(subject);

            //activity log
            await _customerActivityService.InsertActivityAsync("AddNewSubject",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.AddNewSubject"), subject.Id), subject);

            //locales
            await UpdateLocalesAsync(subject, model);


//{{StoreMappingSaveMethodCallHere}}

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Subjects.Subjects.Added"));

            if (!continueEditing)
                return RedirectToAction("List");

            return RedirectToAction("Edit", new { id = subject.Id });
        }

        //prepare model
        model = await _subjectModelFactory.PrepareSubjectModelAsync(model, null, true);

        //if we got this far, something failed, redisplay form
        return View(model);
    }

    [CheckPermission(StandardPermission.Subjects.MANAGE_SUBJECTS)]
    public virtual async Task<IActionResult> Edit(int id)
    {
        //try to get a subject with the specified id
        var subject = await _subjectService.GetSubjectByIdAsync(id);
        if (subject == null)
            return RedirectToAction("List");

        //prepare model
        var model = await _subjectModelFactory.PrepareSubjectModelAsync(null, subject);

        return View(model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    [CheckPermission(StandardPermission.Subjects.MANAGE_SUBJECTS)]
    public virtual async Task<IActionResult> Edit(SubjectModel model, bool continueEditing)
    {
        //try to get a subject with the specified id
        var subject = await _subjectService.GetSubjectByIdAsync(model.Id);
        if (subject == null)
            return RedirectToAction("List");

        if (ModelState.IsValid)
        {
            subject = model.ToEntity(subject);
            await _subjectService.UpdateSubjectAsync(subject);

            //activity log
            await _customerActivityService.InsertActivityAsync("EditSubject",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.EditSubject"), subject.Id), subject);

            //locales
            await UpdateLocalesAsync(subject, model);


//{{StoreMappingSaveMethodCallHere}}

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Subjects.Subjects.Updated"));

            if (!continueEditing)
                return RedirectToAction("List");

            return RedirectToAction("Edit", new { id = subject.Id });
        }

        //prepare model
        model = await _subjectModelFactory.PrepareSubjectModelAsync(model, subject, true);

        //if we got this far, something failed, redisplay form
        return View(model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Subjects.MANAGE_SUBJECTS)]
    public virtual async Task<IActionResult> Delete(int id)
    {
        //try to get a subject with the specified id
        var subject = await _subjectService.GetSubjectByIdAsync(id);
        if (subject == null)
            return RedirectToAction("List");

        try
        {
            await _subjectService.DeleteSubjectAsync(subject);

            //activity log
            await _customerActivityService.InsertActivityAsync("DeleteSubject",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.DeleteSubject"), subject.Id), subject);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Subjects.Subjects.Deleted"));

            return RedirectToAction("List");
        }
        catch (Exception exc)
        {
            await _notificationService.ErrorNotificationAsync(exc);
            return RedirectToAction("Edit", new { id = subject.Id });
        }
    }

        [HttpPost]
    [CheckPermission(StandardPermission.Subjects.MANAGE_SUBJECTS)]
    public virtual async Task<IActionResult> ActivateSelected(ICollection<int> selectedIds)
    {
        if (selectedIds == null || !selectedIds.Any())
            return NoContent();

        var subjects = await _subjectService.GetSubjectsByIdsAsync([.. selectedIds]);
        foreach (var subject in subjects)
        {
            subject.IsActive = true;
            await _subjectService.UpdateSubjectAsync(subject);
        }

        return Json(new { Result = true });
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Subjects.MANAGE_SUBJECTS)]
    public virtual async Task<IActionResult> DeactivateSelected(ICollection<int> selectedIds)
    {
        if (selectedIds == null || !selectedIds.Any())
            return NoContent();

        var subjects = await _subjectService.GetSubjectsByIdsAsync(selectedIds.ToArray());
        foreach (var subject in subjects)
        {
            subject.IsActive = false;
            await _subjectService.UpdateSubjectAsync(subject);
        }

        return Json(new { Result = true });
    }

    #endregion
}