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
    protected readonly ISectionModelFactory _sectionModelFactory;
    protected readonly ISectionService _sectionService;

    #endregion

    #region Ctor

    public GradeController(
        IGradeModelFactory gradeModelFactory,
        IGradeService gradeService,
        ICustomerActivityService customerActivityService,
        ILocalizationService localizationService,
        ILocalizedEntityService localizedEntityService,
        INotificationService notificationService,
        IStoreMappingService storeMappingService,
        ISectionModelFactory sectionModelFactory,
        ISectionService sectionService
        )
    {
        _gradeModelFactory = gradeModelFactory;
        _gradeService = gradeService;
        _customerActivityService = customerActivityService;
        _localizationService = localizationService;
        _localizedEntityService = localizedEntityService;
        _notificationService = notificationService;
        _storeMappingService = storeMappingService;
        _sectionModelFactory = sectionModelFactory;
        _sectionService = sectionService;
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

    protected virtual async Task UpdateSectionLocalesAsync(Section section, SectionModel model)
    {
        foreach (var localized in model.Locales)
        {
            await _localizedEntityService.SaveLocalizedValueAsync(section,
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

    #region Sections

    [CheckPermission(StandardPermission.GradeManagement.MANAGE_SECTIONS)]
    public virtual async Task<IActionResult> SectionList()
    {
        //prepare model
        var model = await _sectionModelFactory.PrepareSectionSearchModelAsync(new SectionSearchModel());

        return View(model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.GradeManagement.MANAGE_SECTIONS)]
    public virtual async Task<IActionResult> SectionGridList(SectionSearchModel searchModel)
    {
        //prepare model
        var model = await _sectionModelFactory.PrepareSectionListModelAsync(searchModel);

        return Json(model);
    }

    [CheckPermission(StandardPermission.GradeManagement.MANAGE_SECTIONS)]
    public virtual async Task<IActionResult> SectionCreate()
    {
        //prepare model
        var model = await _sectionModelFactory.PrepareSectionModelAsync(new SectionModel(), null);

        return View(model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    [CheckPermission(StandardPermission.GradeManagement.MANAGE_SECTIONS)]
    public virtual async Task<IActionResult> SectionCreate(SectionModel model, bool continueEditing)
    {
        if (ModelState.IsValid)
        {
            var section = model.ToEntity<Section>();
            await _sectionService.InsertSectionAsync(section);

            //activity log
            await _customerActivityService.InsertActivityAsync("AddNewSection",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.AddNewSection"), section.Id), section);

            //locales
            await UpdateSectionLocalesAsync(section, model);


            //{{StoreMappingSaveMethodCallHere}}

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Sections.Sections.Added"));

            if (!continueEditing)
                return RedirectToAction("List");

            return RedirectToAction("Edit", new { id = section.Id });
        }

        //prepare model
        model = await _sectionModelFactory.PrepareSectionModelAsync(model, null, true);

        //if we got this far, something failed, redisplay form
        return View(model);
    }

    [CheckPermission(StandardPermission.GradeManagement.MANAGE_SECTIONS)]
    public virtual async Task<IActionResult> SectionEdit(int id)
    {
        //try to get a section with the specified id
        var section = await _sectionService.GetSectionByIdAsync(id);
        if (section == null)
            return RedirectToAction("List");

        //prepare model
        var model = await _sectionModelFactory.PrepareSectionModelAsync(null, section);

        return View(model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    [CheckPermission(StandardPermission.GradeManagement.MANAGE_SECTIONS)]
    public virtual async Task<IActionResult> SectionEdit(SectionModel model, bool continueEditing)
    {
        //try to get a section with the specified id
        var section = await _sectionService.GetSectionByIdAsync(model.Id);
        if (section == null)
            return RedirectToAction("List");

        if (ModelState.IsValid)
        {
            section = model.ToEntity(section);
            await _sectionService.UpdateSectionAsync(section);

            //activity log
            await _customerActivityService.InsertActivityAsync("EditSection",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.EditSection"), section.Id), section);

            //locales
            await UpdateSectionLocalesAsync(section, model);


            //{{StoreMappingSaveMethodCallHere}}

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Sections.Sections.Updated"));

            if (!continueEditing)
                return RedirectToAction("List");

            return RedirectToAction("Edit", new { id = section.Id });
        }

        //prepare model
        model = await _sectionModelFactory.PrepareSectionModelAsync(model, section, true);

        //if we got this far, something failed, redisplay form
        return View(model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.GradeManagement.MANAGE_SECTIONS)]
    public virtual async Task<IActionResult> SectionDelete(int id)
    {
        //try to get a section with the specified id
        var section = await _sectionService.GetSectionByIdAsync(id);
        if (section == null)
            return RedirectToAction("List");

        try
        {
            await _sectionService.DeleteSectionAsync(section);

            //activity log
            await _customerActivityService.InsertActivityAsync("DeleteSection",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.DeleteSection"), section.Id), section);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Sections.Sections.Deleted"));

            return RedirectToAction("List");
        }
        catch (Exception exc)
        {
            await _notificationService.ErrorNotificationAsync(exc);
            return RedirectToAction("Edit", new { id = section.Id });
        }
    }

    #endregion
}