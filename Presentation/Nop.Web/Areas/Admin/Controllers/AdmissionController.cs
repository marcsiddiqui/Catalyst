using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Nop.Core;
using Nop.Core.Domain.Admissions;
using Nop.Core.Domain.GenericDropDowns;
using Nop.Services.Admissions;
using Nop.Services.Common;
using Nop.Services.GenericDropDowns;
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
    protected const int StudentPictureAdmissionDocumentTypeId = -1;
    protected const string StudentPictureAttachmentModuleName = "AdmissionStudentPicture";
    protected const string StudentPictureFileNamePrefix = "StudentPicture";

    #region Fields

    protected readonly IAdmissionModelFactory _admissionModelFactory;
    protected readonly IAdmissionDocumentService _admissionDocumentService;
    protected readonly IAdmissionService _admissionService;
    protected readonly IAttachmentFileService _attachmentFileService;
    protected readonly ICustomerActivityService _customerActivityService;
    protected readonly IGenericDropDownOptionService _genericDropDownOptionService;
    protected readonly ILocalizationService _localizationService;
    protected readonly ILocalizedEntityService _localizedEntityService;
    protected readonly INotificationService _notificationService;
    protected readonly IStoreMappingService _storeMappingService;
    protected readonly IWorkContext _workContext;

    #endregion

    #region Ctor

    public AdmissionController(
        IAdmissionModelFactory admissionModelFactory,
        IAdmissionDocumentService admissionDocumentService,
        IAdmissionService admissionService,
        IAttachmentFileService attachmentFileService,
        ICustomerActivityService customerActivityService,
        IGenericDropDownOptionService genericDropDownOptionService,
        ILocalizationService localizationService,
        ILocalizedEntityService localizedEntityService,
        INotificationService notificationService,
        IStoreMappingService storeMappingService,
        IWorkContext workContext)
    {
        _admissionModelFactory = admissionModelFactory;
        _admissionDocumentService = admissionDocumentService;
        _admissionService = admissionService;
        _attachmentFileService = attachmentFileService;
        _customerActivityService = customerActivityService;
        _genericDropDownOptionService = genericDropDownOptionService;
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
            SRN = await _admissionService.GenerateAdmissionSrnAsync(),
            FirstName = string.Empty,
            LastName = string.Empty,
            Address = string.Empty,
            FatherFullName = string.Empty,
            FatherPhoneNo = string.Empty,
            FatherOfficeAddress = string.Empty,
            FatherOfficePhoneNumber = string.Empty,
            MotherFullName = string.Empty,
            MotherPhoneNo = string.Empty,
            MotherOfficeAddress = string.Empty,
            MotherOfficePhoneNumber = string.Empty,
            GuardianFullName = string.Empty,
            GuardianPhoneNo = string.Empty,
            DateOfBirth = now,
            FatherDateOfBirth = now,
            MotherDateOfBirth = now,
            GuardianDateOfBirth = now,
            CreatedBy = customer?.Id ?? 0,
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

    protected virtual async Task EnsureAdmissionSrnAsync(Admission admission)
    {
        if (admission == null || admission.SRN > 0)
            return;

        admission.SRN = await _admissionService.GenerateAdmissionSrnAsync();
    }

    protected virtual string ValidateAdmissionStep(AdmissionModel model, int step)
    {
        if (string.IsNullOrWhiteSpace(model.FormNo))
            return "Form no. is required.";

        return step switch
        {
            1 when model.GradeId <= 0 => "Grade is required.",
            2 when string.IsNullOrWhiteSpace(model.FirstName) => "First name is required.",
            2 when string.IsNullOrWhiteSpace(model.LastName) => "Last name is required.",
            2 when string.IsNullOrWhiteSpace(model.Address) => "Address is required.",
            5 when string.IsNullOrWhiteSpace(model.GuardianFullName) => "Guardian full name is required.",
            5 when string.IsNullOrWhiteSpace(model.GuardianPhoneNo) => "Guardian phone no. is required.",
            _ => string.Empty
        };
    }

    protected virtual bool IsAllowedAdmissionDocument(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return false;

        var extension = Path.GetExtension(file.FileName)?.ToLowerInvariant();
        var contentType = file.ContentType?.ToLowerInvariant();

        return extension == ".pdf" ||
            contentType?.StartsWith("image/") == true ||
            new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp", ".tif", ".tiff" }.Contains(extension);
    }

    protected virtual bool IsAllowedAdmissionPicture(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return false;

        var extension = Path.GetExtension(file.FileName)?.ToLowerInvariant();
        var contentType = file.ContentType?.ToLowerInvariant();

        return contentType?.StartsWith("image/") == true ||
            new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp", ".tif", ".tiff" }.Contains(extension);
    }

    protected virtual async Task<AdmissionDocument> GetAdmissionStudentPictureAsync(int admissionId)
    {
        var pictures = await _admissionDocumentService.GetAllAdmissionDocumentsAsync(admissionId: admissionId);
        var studentPicturePath = $"/{StudentPictureAttachmentModuleName}/{admissionId}/";

        return pictures.FirstOrDefault(document => document.AdmissionDocumentTypeId == StudentPictureAdmissionDocumentTypeId)
            ?? pictures.FirstOrDefault(document => document.FilePath?.Contains(studentPicturePath, StringComparison.InvariantCultureIgnoreCase) == true);
    }

    protected virtual async Task<IList<AdmissionRequiredDocumentModel>> PrepareAdmissionRequiredDocumentsAsync(Admission admission)
    {
        if (admission == null || admission.GradeId <= 0)
            return new List<AdmissionRequiredDocumentModel>();

        var requirements = await _admissionService.GetAllAdmissionGradeDocumentRequirementsAsync(gradeId: admission.GradeId);
        var documents = await _admissionDocumentService.GetAllAdmissionDocumentsAsync(admissionId: admission.Id);
        var documentTypes = await _genericDropDownOptionService.GetGenericDropDownOptionsByEntityAsync(GenericDropdownEntity.AdmissionDocumentType);
        var documentTypeNames = documentTypes.ToDictionary(option => option.Value, option => option.Text);

        return requirements.Select(requirement =>
        {
            var document = documents.FirstOrDefault(item => item.AdmissionDocumentTypeId == requirement.AdmissionDocumentTypeId);

            return new AdmissionRequiredDocumentModel
            {
                AdmissionDocumentTypeId = requirement.AdmissionDocumentTypeId,
                AdmissionDocumentTypeName = documentTypeNames.TryGetValue(requirement.AdmissionDocumentTypeId, out var name)
                    ? name
                    : requirement.AdmissionDocumentTypeId.ToString(),
                IsRequired = requirement.IsRequired,
                AdmissionDocumentId = document?.Id ?? 0,
                FileName = document?.FileName,
                PreviewUrl = document != null ? Url.Action("PreviewDocument", "Admission", new { id = document.Id }) : null
            };
        }).ToList();
    }

    protected virtual async Task<string> ValidateAdmissionDocumentsAsync(Admission admission)
    {
        var requirements = await _admissionService.GetAllAdmissionGradeDocumentRequirementsAsync(gradeId: admission.GradeId);
        var requiredDocumentTypeIds = requirements.Where(requirement => requirement.IsRequired).Select(requirement => requirement.AdmissionDocumentTypeId).ToList();

        if (!requiredDocumentTypeIds.Any())
            return string.Empty;

        var documents = await _admissionDocumentService.GetAllAdmissionDocumentsAsync(
            admissionId: admission.Id,
            admissionDocumentTypeIds: requiredDocumentTypeIds);

        var uploadedDocumentTypeIds = documents.Select(document => document.AdmissionDocumentTypeId).Distinct().ToList();
        var missingDocumentTypeIds = requiredDocumentTypeIds.Except(uploadedDocumentTypeIds).ToList();

        if (!missingDocumentTypeIds.Any())
            return string.Empty;

        var documentTypes = await _genericDropDownOptionService.GetGenericDropDownOptionsByEntityAsync(GenericDropdownEntity.AdmissionDocumentType);
        var missingDocumentNames = documentTypes
            .Where(option => missingDocumentTypeIds.Contains(option.Value))
            .Select(option => option.Text)
            .ToList();

        var missingDocuments = missingDocumentNames.Any()
            ? missingDocumentNames
            : missingDocumentTypeIds.Select(id => id.ToString()).ToList();

        return $"Please upload required document(s): {string.Join(", ", missingDocuments)}.";
    }

    protected virtual void ApplyAdmissionStep(Admission admission, AdmissionModel model, int step)
    {
        admission.FormNo = model.FormNo?.Trim();

        switch (step)
        {
            case 1:
                admission.StatusId = model.StatusId;
                admission.AcademicYearId = model.AcademicYearId;
                admission.GradeId = model.GradeId;
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
                admission.Address = model.Address;
                admission.SiblingsCount = model.SiblingsCount;
                admission.NoInSiblings = model.NoInSiblings;
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
                admission.FatherOfficeAddress = model.FatherOfficeAddress;
                admission.FatherOfficePhoneNumber = model.FatherOfficePhoneNumber;
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
                admission.MotherOfficeAddress = model.MotherOfficeAddress;
                admission.MotherOfficePhoneNumber = model.MotherOfficePhoneNumber;
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

        return Json(new
        {
            success = true,
            created = admission == null,
            id = admission?.Id ?? 0,
            srn = admission?.SRN ?? 0,
            editUrl = admission != null ? Url.Action("Edit", new { id = admission.Id }) : null
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

        if (step == 6)
        {
            var documentError = await ValidateAdmissionDocumentsAsync(admission);
            if (!string.IsNullOrWhiteSpace(documentError))
                return Json(new { success = false, message = documentError });
        }

        ApplyAdmissionStep(admission, model, step);
        if (step == 1)
            await EnsureAdmissionSrnAsync(admission);

        await TouchAdmissionAsync(admission);
        await _admissionService.UpdateAdmissionAsync(admission);

        return Json(new
        {
            success = true,
            id = admission.Id,
            srn = admission.SRN,
            editUrl = Url.Action("Edit", new { id = admission.Id })
        });
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Admissions.MANAGE_ADMISSIONS)]
    public virtual async Task<IActionResult> RequiredDocuments(int admissionId)
    {
        var admission = await _admissionService.GetAdmissionByIdAsync(admissionId);
        if (admission == null)
            return Json(new { success = false, message = "Admission not found." });

        return Json(new
        {
            success = true,
            documents = await PrepareAdmissionRequiredDocumentsAsync(admission)
        });
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Admissions.MANAGE_ADMISSIONS)]
    public virtual async Task<IActionResult> StudentPicture(int admissionId)
    {
        var admission = await _admissionService.GetAdmissionByIdAsync(admissionId);
        if (admission == null)
            return Json(new { success = false, message = "Admission not found." });

        var picture = await GetAdmissionStudentPictureAsync(admission.Id);

        return Json(new
        {
            success = true,
            documentId = picture?.Id ?? 0,
            fileName = picture?.FileName,
            previewUrl = picture != null ? Url.Action("PreviewDocument", "Admission", new { id = picture.Id }) : null
        });
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Admissions.MANAGE_ADMISSIONS)]
    public virtual async Task<IActionResult> UploadStudentPicture(int admissionId, IFormFile file)
    {
        var admission = await _admissionService.GetAdmissionByIdAsync(admissionId);
        if (admission == null)
            return Json(new { success = false, message = "Admission not found." });

        if (!IsAllowedAdmissionPicture(file))
            return Json(new { success = false, message = "Only image files are allowed." });

        var existingPicture = await GetAdmissionStudentPictureAsync(admission.Id);
        if (existingPicture != null)
        {
            await _attachmentFileService.DeleteAttachmentAsync(existingPicture.FilePath);
            await _admissionDocumentService.DeleteAdmissionDocumentAsync(existingPicture);
        }

        var savedFile = await _attachmentFileService.SaveAttachmentAsync(
            file,
            StudentPictureAttachmentModuleName,
            admission.Id,
            StudentPictureFileNamePrefix);

        var admissionDocument = new AdmissionDocument
        {
            AdmissionId = admission.Id,
            AdmissionDocumentTypeId = StudentPictureAdmissionDocumentTypeId,
            FileName = savedFile.FileName,
            FilePath = savedFile.FilePath,
            Deleted = false
        };

        await _admissionDocumentService.InsertAdmissionDocumentAsync(admissionDocument);

        return Json(new
        {
            success = true,
            documentId = admissionDocument.Id,
            fileName = admissionDocument.FileName,
            previewUrl = Url.Action("PreviewDocument", "Admission", new { id = admissionDocument.Id })
        });
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Admissions.MANAGE_ADMISSIONS)]
    public virtual async Task<IActionResult> UploadDocument(int admissionId, int admissionDocumentTypeId, IFormFile file)
    {
        var admission = await _admissionService.GetAdmissionByIdAsync(admissionId);
        if (admission == null)
            return Json(new { success = false, message = "Admission not found." });

        if (!IsAllowedAdmissionDocument(file))
            return Json(new { success = false, message = "Only image files and PDF documents are allowed." });

        var requirements = await _admissionService.GetAllAdmissionGradeDocumentRequirementsAsync(
            gradeId: admission.GradeId,
            admissionDocumentTypeId: admissionDocumentTypeId);

        if (!requirements.Any())
            return Json(new { success = false, message = "Document type is not required for the selected grade." });

        var existingDocuments = await _admissionDocumentService.GetAllAdmissionDocumentsAsync(
            admissionId: admission.Id,
            admissionDocumentTypeId: admissionDocumentTypeId);

        foreach (var existingDocument in existingDocuments)
        {
            await _attachmentFileService.DeleteAttachmentAsync(existingDocument.FilePath);
            await _admissionDocumentService.DeleteAdmissionDocumentAsync(existingDocument);
        }

        var documentTypes = await _genericDropDownOptionService.GetGenericDropDownOptionsByEntityAsync(GenericDropdownEntity.AdmissionDocumentType);
        var documentTypeName = documentTypes.FirstOrDefault(option => option.Value == admissionDocumentTypeId)?.Text
            ?? admissionDocumentTypeId.ToString();

        var savedFile = await _attachmentFileService.SaveAttachmentAsync(file, "Admission", admission.Id, documentTypeName);
        var admissionDocument = new AdmissionDocument
        {
            AdmissionId = admission.Id,
            AdmissionDocumentTypeId = admissionDocumentTypeId,
            FileName = savedFile.FileName,
            FilePath = savedFile.FilePath,
            Deleted = false
        };

        await _admissionDocumentService.InsertAdmissionDocumentAsync(admissionDocument);

        return Json(new
        {
            success = true,
            documentId = admissionDocument.Id,
            fileName = admissionDocument.FileName,
            previewUrl = Url.Action("PreviewDocument", "Admission", new { id = admissionDocument.Id })
        });
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Admissions.MANAGE_ADMISSIONS)]
    public virtual async Task<IActionResult> DeleteDocument(int id)
    {
        var admissionDocument = await _admissionDocumentService.GetAdmissionDocumentByIdAsync(id);
        if (admissionDocument == null)
            return Json(new { success = false, message = "Document not found." });

        await _attachmentFileService.DeleteAttachmentAsync(admissionDocument.FilePath);
        await _admissionDocumentService.DeleteAdmissionDocumentAsync(admissionDocument);

        return Json(new { success = true });
    }

    [CheckPermission(StandardPermission.Admissions.MANAGE_ADMISSIONS)]
    public virtual async Task<IActionResult> PreviewDocument(int id)
    {
        var admissionDocument = await _admissionDocumentService.GetAdmissionDocumentByIdAsync(id);
        if (admissionDocument == null)
            return RedirectToAction("List");

        var physicalPath = _attachmentFileService.GetAttachmentPhysicalPath(admissionDocument.FilePath);
        if (!System.IO.File.Exists(physicalPath))
            return NotFound();

        var contentTypeProvider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
        if (!contentTypeProvider.TryGetContentType(physicalPath, out var contentType))
            contentType = "application/octet-stream";

        return PhysicalFile(physicalPath, contentType);
    }

    [CheckPermission(StandardPermission.Admissions.MANAGE_ADMISSIONS)]
    public virtual async Task<IActionResult> PreviewStudentPicture(int admissionId)
    {
        var admission = await _admissionService.GetAdmissionByIdAsync(admissionId);
        if (admission == null)
            return NotFound();

        var studentPicture = await GetAdmissionStudentPictureAsync(admission.Id);
        if (studentPicture == null)
            return NotFound();

        return RedirectToAction("PreviewDocument", new { id = studentPicture.Id });
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
            await EnsureAdmissionSrnAsync(admission);
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
        ViewBag.StudentPictureUrl = Url.Action("PreviewStudentPicture", "Admission", new { admissionId = admission.Id });

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
