using Microsoft.AspNetCore.Mvc;
using Nop.Core;
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
    protected readonly IWorkContext _workContext;

    #endregion

    #region Ctor

    public AdmissionController(
        IAdmissionModelFactory admissionModelFactory,
        IAdmissionService admissionService,
        ICustomerActivityService customerActivityService,
        ILocalizationService localizationService,
        ILocalizedEntityService localizedEntityService,
        INotificationService notificationService,
        IStoreMappingService storeMappingService,
        IWorkContext workContext)
    {
        _admissionModelFactory = admissionModelFactory;
        _admissionService = admissionService;
        _customerActivityService = customerActivityService;
        _localizationService = localizationService;
        _localizedEntityService = localizedEntityService;
        _notificationService = notificationService;
        _storeMappingService = storeMappingService;
        _workContext = workContext;
    }

    #endregion

    #region Utilities

    protected virtual async Task<Admission> GetAdmissionByFormNoAsync(string formNo)
    {
        if (string.IsNullOrWhiteSpace(formNo))
            return null;

        var admissions = await _admissionService.GetAllAdmissionsAsync(formNo: formNo.Trim(), pageSize: 1);

        return admissions.FirstOrDefault();
    }

    protected virtual async Task<Admission> CreateAdmissionDraftAsync(string formNo)
    {
        var customer = await _workContext.GetCurrentCustomerAsync();
        var now = DateTime.UtcNow;
        var admission = new Admission
        {
            FormNo = formNo.Trim(),
            FirstName = string.Empty,
            LastName = string.Empty,
            FatherFullName = string.Empty,
            FatherPhoneNo = string.Empty,
            MotherFullName = string.Empty,
            MotherPhoneNo = string.Empty,
            GuardianFullName = string.Empty,
            GuardianPhoneNo = string.Empty,
            DateOfBirth = now,
            FatherDateOfBirth = now,
            MotherDateOfBirth = now,
            GuardianDateOfBirth = now,
            Createdby = customer?.Id ?? 0,
            CreatedOnUtc = now,
            UpdatedBy = customer?.Id ?? 0,
            UpdatedOnUtc = now
        };

        await _admissionService.InsertAdmissionAsync(admission);

        return admission;
    }

    protected virtual async Task TouchAdmissionAsync(Admission admission)
    {
        var customer = await _workContext.GetCurrentCustomerAsync();

        admission.UpdatedBy = customer?.Id ?? 0;
        admission.UpdatedOnUtc = DateTime.UtcNow;
    }

    protected virtual string ValidateAdmissionStep(AdmissionModel model, int step)
    {
        if (string.IsNullOrWhiteSpace(model.FormNo))
            return "Form no. is required.";

        return step switch
        {
            2 when string.IsNullOrWhiteSpace(model.FirstName) => "First name is required.",
            2 when string.IsNullOrWhiteSpace(model.LastName) => "Last name is required.",
            3 when string.IsNullOrWhiteSpace(model.FatherFullName) => "Father full name is required.",
            3 when string.IsNullOrWhiteSpace(model.FatherPhoneNo) => "Father phone no. is required.",
            4 when string.IsNullOrWhiteSpace(model.MotherFullName) => "Mother full name is required.",
            4 when string.IsNullOrWhiteSpace(model.MotherPhoneNo) => "Mother phone no. is required.",
            5 when string.IsNullOrWhiteSpace(model.GuardianFullName) => "Guardian full name is required.",
            5 when string.IsNullOrWhiteSpace(model.GuardianPhoneNo) => "Guardian phone no. is required.",
            _ => string.Empty
        };
    }

    protected virtual void ApplyAdmissionStep(Admission admission, AdmissionModel model, int step)
    {
        admission.FormNo = model.FormNo?.Trim();

        switch (step)
        {
            case 1:
                admission.StatusId = model.StatusId;
                break;
            case 2:
                admission.StatusId = model.StatusId;
                admission.FirstName = model.FirstName;
                admission.MiddleName = model.MiddleName;
                admission.LastName = model.LastName;
                admission.CNIC = model.CNIC;
                admission.PreviousSchoolId = model.PreviousSchoolId;
                admission.IdentificationMark = model.IdentificationMark;
                admission.DateOfBirth = model.DateOfBirth;
                admission.BirthCity = model.BirthCity;
                admission.Allergies = model.Allergies;
                admission.MedicalNotes = model.MedicalNotes;
                admission.MontherTongue = model.MontherTongue;
                admission.Nationality = model.Nationality;
                admission.Religion = model.Religion;
                admission.BloodGroup = model.BloodGroup;
                admission.Caste = model.Caste;
                admission.GuardianTypeId = model.GuardianTypeId;
                break;
            case 3:
                admission.FatherFullName = model.FatherFullName;
                admission.FatherCNIC = model.FatherCNIC;
                admission.FatherDateOfBirth = model.FatherDateOfBirth;
                admission.FatherIsDeceased = model.FatherIsDeceased;
                admission.FatherQaulification = model.FatherQaulification;
                admission.FatherPhoneNo = model.FatherPhoneNo;
                admission.FatherProfession = model.FatherProfession;
                admission.FatherMonthlyIncome = model.FatherMonthlyIncome;
                admission.Father_MontherTongue = model.Father_MontherTongue;
                admission.FatherNationality = model.FatherNationality;
                admission.FatherReligion = model.FatherReligion;
                admission.FatherBloodGroup = model.FatherBloodGroup;
                admission.FatherCaste = model.FatherCaste;
                break;
            case 4:
                admission.MotherFullName = model.MotherFullName;
                admission.MotherCNIC = model.MotherCNIC;
                admission.MotherDateOfBirth = model.MotherDateOfBirth;
                admission.MotherIsDeceased = model.MotherIsDeceased;
                admission.MotherQaulification = model.MotherQaulification;
                admission.MotherPhoneNo = model.MotherPhoneNo;
                admission.MotherProfession = model.MotherProfession;
                admission.MotherMonthlyIncome = model.MotherMonthlyIncome;
                admission.Mother_MontherTongue = model.Mother_MontherTongue;
                admission.MotherNationality = model.MotherNationality;
                admission.MotherReligion = model.MotherReligion;
                admission.MotherBloodGroup = model.MotherBloodGroup;
                admission.MotherCaste = model.MotherCaste;
                break;
            case 5:
                admission.GuardianFullName = model.GuardianFullName;
                admission.GuardianCNIC = model.GuardianCNIC;
                admission.GuardianDateOfBirth = model.GuardianDateOfBirth;
                admission.GuardianIsDeceased = model.GuardianIsDeceased;
                admission.GuardianQaulification = model.GuardianQaulification;
                admission.GuardianPhoneNo = model.GuardianPhoneNo;
                admission.GuardianProfession = model.GuardianProfession;
                admission.GuardianMonthlyIncome = model.GuardianMonthlyIncome;
                admission.Guardian_MontherTongue = model.Guardian_MontherTongue;
                admission.GuardianNationality = model.GuardianNationality;
                admission.GuardianReligion = model.GuardianReligion;
                admission.GuardianBloodGroup = model.GuardianBloodGroup;
                admission.GuardianCaste = model.GuardianCaste;
                break;
        }
    }



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
    public virtual async Task<IActionResult> LoadByFormNo(string formNo)
    {
        if (string.IsNullOrWhiteSpace(formNo))
            return Json(new { success = false, message = "Form no. is required." });

        var admission = await GetAdmissionByFormNoAsync(formNo);
        var created = false;

        if (admission == null)
        {
            admission = await CreateAdmissionDraftAsync(formNo);
            created = true;
        }

        return Json(new
        {
            success = true,
            created,
            id = admission.Id,
            editUrl = Url.Action("Edit", new { id = admission.Id })
        });
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Admissions.MANAGE_ADMISSIONS)]
    public virtual async Task<IActionResult> SaveStep(AdmissionModel model, int step)
    {
        var error = ValidateAdmissionStep(model, step);
        if (!string.IsNullOrWhiteSpace(error))
            return Json(new { success = false, message = error });

        var admission = model.Id > 0
            ? await _admissionService.GetAdmissionByIdAsync(model.Id)
            : await GetAdmissionByFormNoAsync(model.FormNo);

        if (admission == null)
            admission = await CreateAdmissionDraftAsync(model.FormNo);

        var duplicateAdmission = await GetAdmissionByFormNoAsync(model.FormNo);
        if (duplicateAdmission != null && duplicateAdmission.Id != admission.Id)
            return Json(new { success = false, message = "Another admission already exists with this form no." });

        ApplyAdmissionStep(admission, model, step);
        await TouchAdmissionAsync(admission);
        await _admissionService.UpdateAdmissionAsync(admission);

        return Json(new
        {
            success = true,
            id = admission.Id,
            editUrl = Url.Action("Edit", new { id = admission.Id })
        });
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

    [CheckPermission(StandardPermission.Admissions.MANAGE_ADMISSIONS)]
    public virtual async Task<IActionResult> Details(int id)
    {
        var admission = await _admissionService.GetAdmissionByIdAsync(id);
        if (admission == null)
            return RedirectToAction("List");

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
