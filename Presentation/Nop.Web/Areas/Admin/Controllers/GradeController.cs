using Markdig.Extensions.Tables;

using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

using Nop.Core;
using Nop.Core.Domain.Admissions;
using Nop.Core.Domain.GradeManagement;
using Nop.Services.Admissions;
using Nop.Services.GradeManagement;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Services.Stores;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.Admissions;
using Nop.Web.Areas.Admin.Models.GradeManagement;
using Nop.Web.Framework.Mvc;
using Nop.Web.Framework.Mvc.Filters;

using System.Configuration;

namespace Nop.Web.Areas.Admin.Controllers;

public partial class GradeController : BaseAdminController
{
    protected static partial class GradeCopySetupItems
    {
        public const string GradeSubjectMapping = "GradeSubjectMapping";
        public const string AdmissionDocumentRequirements = "AdmissionDocumentRequirements";
    }

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
    protected readonly IWorkContext _workContext;
    protected readonly IAdmissionService _admissionService;
    protected readonly IAdmissionModelFactory _admissionModelFactory;

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
        ISectionService sectionService,
        IWorkContext workContext,
        IAdmissionService admissionService,
        IAdmissionModelFactory admissionModelFactory
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
        _workContext = workContext;
        _admissionService = admissionService;
        _admissionModelFactory = admissionModelFactory;
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

    protected virtual IList<string> GetSelectedGradeCopySetupItems(GradeCopySetupModel model)
    {
        var items = new List<string>();

        if (model.CopyGradeSubjectMapping)
            items.Add(GradeCopySetupItems.GradeSubjectMapping);

        if (model.CopyAdmissionDocumentRequirements)
            items.Add(GradeCopySetupItems.AdmissionDocumentRequirements);

        return items;
    }

    protected virtual async Task<IList<string>> CopyGradeSetupAsync(int fromGradeId, int toGradeId, IList<string> itemsToCopy, bool replaceExisting)
    {
        var errors = new List<string>();

        if (fromGradeId <= 0)
            errors.Add("Please select a grade to copy from.");

        if (toGradeId <= 0)
            errors.Add("Destination grade is required.");

        if (fromGradeId == toGradeId)
            errors.Add("Source and destination grades must be different.");

        if (itemsToCopy == null || !itemsToCopy.Any())
            errors.Add("Please select at least one item to copy.");

        if (errors.Any())
            return errors;

        var fromGrade = await _gradeService.GetGradeByIdAsync(fromGradeId);
        var toGrade = await _gradeService.GetGradeByIdAsync(toGradeId);

        if (fromGrade == null)
            errors.Add("Source grade not found.");

        if (toGrade == null)
            errors.Add("Destination grade not found.");

        if (errors.Any())
            return errors;

        if (itemsToCopy.Contains(GradeCopySetupItems.GradeSubjectMapping))
        {
            if (replaceExisting)
            {
                var existingMappings = await _gradeService.GetAllGradeSubjectMappingsAsync(gradeId: toGradeId);
                if (existingMappings.Any())
                    await _gradeService.DeleteGradeSubjectMappingAsync(existingMappings);
            }

            var sourceMappings = await _gradeService.GetAllGradeSubjectMappingsAsync(gradeId: fromGradeId);
            var copiedMappings = sourceMappings.Select(mapping => new GradeSubjectMapping
            {
                GradeId = toGradeId,
                SubjectId = mapping.SubjectId,
                SectionId = mapping.SectionId,
                LabFee = mapping.LabFee,
                Deleted = false
            }).ToList();

            if (copiedMappings.Any())
                await _gradeService.InsertGradeSubjectMappingAsync(copiedMappings);
        }

        if (itemsToCopy.Contains(GradeCopySetupItems.AdmissionDocumentRequirements))
        {
            if (replaceExisting)
            {
                var existingRequirements = await _admissionService.GetAllAdmissionGradeDocumentRequirementsAsync(gradeId: toGradeId);
                if (existingRequirements.Any())
                    await _admissionService.DeleteAdmissionGradeDocumentRequirementAsync(existingRequirements);
            }

            var sourceRequirements = await _admissionService.GetAllAdmissionGradeDocumentRequirementsAsync(gradeId: fromGradeId);
            var copiedRequirements = sourceRequirements.Select(requirement => new AdmissionGradeDocumentRequirement
            {
                GradeId = toGradeId,
                AdmissionDocumentTypeId = requirement.AdmissionDocumentTypeId,
                IsRequired = requirement.IsRequired,
                Deleted = false
            }).ToList();

            if (copiedRequirements.Any())
                await _admissionService.InsertAdmissionGradeDocumentRequirementAsync(copiedRequirements);
        }

        return errors;
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


            //Stores
            await _storeMappingService.SaveStoreMappingsAsync(grade, model.SelectedStoreIds);

            var copyItems = GetSelectedGradeCopySetupItems(model.GradeCopySetupModel);
            if (model.GradeCopySetupModel.FromGradeId > 0 || copyItems.Any())
            {
                var copyErrors = await CopyGradeSetupAsync(model.GradeCopySetupModel.FromGradeId, grade.Id, copyItems, false);
                foreach (var copyError in copyErrors)
                    _notificationService.WarningNotification(copyError);
            }

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
            model.CreatedBy = grade.CreatedBy;
            model.CreatedOnUtc = grade.CreatedOnUtc;
            grade = model.ToEntity(grade);
            await _gradeService.UpdateGradeAsync(grade);

            //activity log
            await _customerActivityService.InsertActivityAsync("EditGrade",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.EditGrade"), grade.Id), grade);

            //locales
            await UpdateLocalesAsync(grade, model);


            //Stores
            await _storeMappingService.SaveStoreMappingsAsync(grade, model.SelectedStoreIds);

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

    #region GradeCopySetup

    [CheckPermission(StandardPermission.GradeManagement.MANAGE_GRADES)]
    public virtual async Task<IActionResult> CopyGradeSetupPopup(int gradeId)
    {
        var grade = await _gradeService.GetGradeByIdAsync(gradeId);
        if (grade == null)
            return RedirectToAction("List");

        var model = await _gradeModelFactory.PrepareGradeCopySetupModelAsync(new GradeCopySetupModel(), gradeId, true);

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [CheckPermission(StandardPermission.GradeManagement.MANAGE_GRADES)]
    public virtual async Task<IActionResult> CopyGradeSetup(GradeCopySetupModel model)
    {
        var errors = await CopyGradeSetupAsync(model.FromGradeId, model.ToGradeId, GetSelectedGradeCopySetupItems(model), true);

        if (errors.Any())
            return Json(new { Result = "error", Errors = errors });

        return Json(new { Result = "success" });
    }

    #endregion

    #region GradeSubjectMapping

    [CheckPermission(StandardPermission.GradeManagement.MANAGE_GRADES)]
    [CheckPermission(StandardPermission.Subjects.MANAGE_SUBJECTS)]
    public virtual async Task<IActionResult> AddOrEditGradeSubjectMappingPopup(int gradeId, int mappingId = 0)
    {
        var gradeSubjectMapping = mappingId > 0 ? (await _gradeService.GetGradeSubjectMappingByIdAsync(mappingId)) : null;
        //prepare model
        var model = await _gradeModelFactory.PrepareGradeSubjectMappingModelAsync(gradeSubjectMapping == null ? new GradeSubjectMappingModel() : null, gradeSubjectMapping);

        return View(model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.GradeManagement.MANAGE_GRADES)]
    [CheckPermission(StandardPermission.Subjects.MANAGE_SUBJECTS)]
    public virtual async Task<IActionResult> GradeSubjectGrid(GradeSubjectSearchModel searchModel)
    {
        //prepare model
        var model = await _gradeModelFactory.PrepareGradeSubjectListModelAsync(searchModel);

        return Json(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public virtual async Task<IActionResult> GradeSubjectMappingSave(GradeSubjectMappingModel model)
    {
        if (model.Id > 0)
        {
            var mapping = await _gradeService.GetGradeSubjectMappingByIdAsync(model.Id);
            if (mapping == null || mapping.Deleted)
                return Json(new { Result = "error", Errors = new[] { "Mapping not found." } });

            if (mapping.SubjectId != model.SubjectId || mapping.SectionId != model.SectionId)
            {
                var exists = await _gradeService.GetAllGradeSubjectMappingsAsync(subjectId: model.SubjectId, sectionId: model.SectionId ?? 0, gradeId: mapping.GradeId);

                if (exists != null && exists.Any())
                    return Json(new { Result = "error", Errors = new[] { "This subject/section combination already exists." } });
            }

            mapping.SubjectId = model.SubjectId;
            mapping.SectionId = model.SectionId;
            mapping.LabFee = model.LabFee;

            await _gradeService.UpdateGradeSubjectMappingAsync(mapping);

            return Json(new { Result = "success" });
        }

        if (model.SelectedSubjectIds == null || !model.SelectedSubjectIds.Any())
            return Json(new { Result = "error", Errors = new[] { "Please select at least one subject." } });

        var errors = new List<string>();

        var sectionIds = (model.SelectedSectionIds != null && model.SelectedSectionIds.Any())
            ? model.SelectedSectionIds
            : new List<int> { 0 };

        foreach (var subjectId in model.SelectedSubjectIds)
        {
            foreach (var sectionId in sectionIds)
            {
                var exists = await _gradeService.GetAllGradeSubjectMappingsAsync(subjectId: subjectId, sectionId: sectionId == 0 ? 0 : sectionId, gradeId: model.GradeId);

                if (exists != null && exists.Any())
                {
                    errors.Add($"Mapping for subject {subjectId} / section {sectionId} already exists.");
                    continue;
                }

                var mapping = new GradeSubjectMapping
                {
                    GradeId = model.GradeId,
                    SubjectId = subjectId,
                    SectionId = sectionId == 0 ? null : (int?)sectionId,
                    LabFee = model.LabFee,
                    Deleted = false
                };

                await _gradeService.InsertGradeSubjectMappingAsync(mapping);
            }
        }

        if (errors.Any() && errors.Count < model.SelectedSubjectIds.Count * sectionIds.Count)
            return Json(new { Result = "success", Warnings = errors });

        if (errors.Any())
            return Json(new { Result = "error", Errors = errors });

        return Json(new { Result = "success" });
    }

    [HttpPost]
    [CheckPermission(StandardPermission.GradeManagement.MANAGE_GRADES)]
    [CheckPermission(StandardPermission.Subjects.MANAGE_SUBJECTS)]
    public virtual async Task<IActionResult> GradeSubjectDelete(int id)
    {
        //try to get a tier price with the specified id
        var gradeSubjectMapping = await _gradeService.GetGradeSubjectMappingByIdAsync(id)
            ?? throw new ArgumentException("No grade subject mapping found with the specified id");

        //try to get a product with the specified id
        var grade = await _gradeService.GetGradeByIdAsync(gradeSubjectMapping.GradeId)
            ?? throw new ArgumentException("No grade found with the specified id");

        await _gradeService.DeleteGradeSubjectMappingAsync(gradeSubjectMapping);

        return new NullJsonResult();
    }

    #endregion

    #region AdmissionGradeDocumentRequirement

    [CheckPermission(StandardPermission.GradeManagement.MANAGE_GRADES)]
    public virtual async Task<IActionResult> AddOrEditAdmissionGradeDocumentRequirementPopup(int gradeId, int requirementId = 0)
    {
        var requirement = requirementId > 0
            ? await _admissionService.GetAdmissionGradeDocumentRequirementByIdAsync(requirementId)
            : null;

        var model = await _admissionModelFactory.PrepareAdmissionGradeDocumentRequirementModelAsync(
            requirement == null ? new AdmissionGradeDocumentRequirementModel { GradeId = gradeId } : null,
            requirement);

        model.GradeId = gradeId;

        return View(model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.GradeManagement.MANAGE_GRADES)]
    public virtual async Task<IActionResult> AdmissionGradeDocumentRequirementGrid(AdmissionGradeDocumentRequirementSearchModel searchModel)
    {
        var model = await _admissionModelFactory.PrepareAdmissionGradeDocumentRequirementListModelAsync(searchModel);

        return Json(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [CheckPermission(StandardPermission.GradeManagement.MANAGE_GRADES)]
    public virtual async Task<IActionResult> AdmissionGradeDocumentRequirementSave(AdmissionGradeDocumentRequirementModel model)
    {
        if (model.Id > 0)
        {
            var requirement = await _admissionService.GetAdmissionGradeDocumentRequirementByIdAsync(model.Id);
            if (requirement == null || requirement.Deleted)
                return Json(new { Result = "error", Errors = new[] { "Document requirement not found." } });

            if (requirement.AdmissionDocumentTypeId != model.AdmissionDocumentTypeId)
            {
                var duplicateRequirements = await _admissionService.GetAllAdmissionGradeDocumentRequirementsAsync(
                    gradeId: requirement.GradeId,
                    admissionDocumentTypeId: model.AdmissionDocumentTypeId);

                if (duplicateRequirements != null && duplicateRequirements.Any(duplicate => duplicate.Id != requirement.Id))
                    return Json(new { Result = "error", Errors = new[] { "This document requirement already exists for the grade." } });
            }

            requirement.AdmissionDocumentTypeId = model.AdmissionDocumentTypeId;
            requirement.IsRequired = model.IsRequired;

            await _admissionService.UpdateAdmissionGradeDocumentRequirementAsync(requirement);

            return Json(new { Result = "success" });
        }

        if (model.SelectedAdmissionDocumentTypeIds == null || !model.SelectedAdmissionDocumentTypeIds.Any())
            return Json(new { Result = "error", Errors = new[] { "Please select at least one document type." } });

        var errors = new List<string>();

        foreach (var documentTypeId in model.SelectedAdmissionDocumentTypeIds.Distinct())
        {
            var duplicateRequirements = await _admissionService.GetAllAdmissionGradeDocumentRequirementsAsync(
                gradeId: model.GradeId,
                admissionDocumentTypeId: documentTypeId);

            if (duplicateRequirements != null && duplicateRequirements.Any())
            {
                errors.Add($"Document type {documentTypeId} already exists for this grade.");
                continue;
            }

            var requirement = new AdmissionGradeDocumentRequirement
            {
                GradeId = model.GradeId,
                AdmissionDocumentTypeId = documentTypeId,
                IsRequired = model.IsRequired,
                Deleted = false
            };

            await _admissionService.InsertAdmissionGradeDocumentRequirementAsync(requirement);
        }

        if (errors.Any() && errors.Count < model.SelectedAdmissionDocumentTypeIds.Distinct().Count())
            return Json(new { Result = "success", Warnings = errors });

        if (errors.Any())
            return Json(new { Result = "error", Errors = errors });

        return Json(new { Result = "success" });
    }

    [HttpPost]
    [CheckPermission(StandardPermission.GradeManagement.MANAGE_GRADES)]
    public virtual async Task<IActionResult> AdmissionGradeDocumentRequirementDelete(int id)
    {
        var requirement = await _admissionService.GetAdmissionGradeDocumentRequirementByIdAsync(id)
            ?? throw new ArgumentException("No admission document requirement found with the specified id");

        var grade = await _gradeService.GetGradeByIdAsync(requirement.GradeId)
            ?? throw new ArgumentException("No grade found with the specified id");

        await _admissionService.DeleteAdmissionGradeDocumentRequirementAsync(requirement);

        return new NullJsonResult();
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
                return RedirectToAction("SectionList");

            return RedirectToAction("SectionEdit", new { id = section.Id });
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
            return RedirectToAction("SectionList");

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
            return RedirectToAction("SectionList");

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
                return RedirectToAction("SectionList");

            return RedirectToAction("SectionEdit", new { id = section.Id });
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
            return RedirectToAction("SectionList");

        try
        {
            await _sectionService.DeleteSectionAsync(section);

            //activity log
            await _customerActivityService.InsertActivityAsync("DeleteSection",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.DeleteSection"), section.Id), section);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Sections.Sections.Deleted"));

            return RedirectToAction("SectionList");
        }
        catch (Exception exc)
        {
            await _notificationService.ErrorNotificationAsync(exc);
            return RedirectToAction("SectionEdit", new { id = section.Id });
        }
    }

    #endregion
}
