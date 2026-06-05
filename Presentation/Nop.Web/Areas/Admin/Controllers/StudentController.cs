using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Media;
using Nop.Core.Domain.Students;
using Nop.Services.Customers;
using Nop.Services.Common;
using Nop.Services.Media;
using Nop.Services.Students;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Services.Stores;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.Students;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Web.Areas.Admin.Controllers;

public partial class StudentController : BaseAdminController
{
    #region Fields

    protected readonly IStudentModelFactory _studentModelFactory;
    protected readonly ICustomerService _customerService;
    protected readonly IAttachmentFileService _attachmentFileService;
    protected readonly IPictureService _pictureService;
    protected readonly IStudentService _studentService;
    protected readonly ICustomerActivityService _customerActivityService;
    protected readonly ILocalizationService _localizationService;
    protected readonly ILocalizedEntityService _localizedEntityService;
    protected readonly INotificationService _notificationService;
    protected readonly IStoreMappingService _storeMappingService;
    protected readonly IStoreContext _storeContext;
    protected readonly IWorkContext _workContext;

    #endregion

    #region Ctor

    public StudentController(
        IStudentModelFactory studentModelFactory,
        ICustomerService customerService,
        IAttachmentFileService attachmentFileService,
        IPictureService pictureService,
        IStudentService studentService,
        ICustomerActivityService customerActivityService,
        ILocalizationService localizationService,
        ILocalizedEntityService localizedEntityService,
        INotificationService notificationService,
        IStoreMappingService storeMappingService,
        IStoreContext storeContext,
        IWorkContext workContext)
    {
        _studentModelFactory = studentModelFactory;
        _customerService = customerService;
        _attachmentFileService = attachmentFileService;
        _pictureService = pictureService;
        _studentService = studentService;
        _customerActivityService = customerActivityService;
        _localizationService = localizationService;
        _localizedEntityService = localizedEntityService;
        _notificationService = notificationService;
        _storeMappingService = storeMappingService;
        _storeContext = storeContext;
        _workContext = workContext;
    }

    #endregion

    #region Utilities
    protected static readonly bool UseAttachmentStorageForStudentPictures = true;
    protected const string StudentPictureAttachmentModuleName = "Student";
    protected const string StudentTemporaryPictureAttachmentModuleName = "StudentTemp";
    protected const string StudentPictureFileNamePrefix = "StudentPicture";
    protected const string StudentTemporaryPicturePathPrefix = "Attachments/StudentTemp/";

    protected virtual bool IsAllowedStudentPicture(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return false;

        var extension = Path.GetExtension(file.FileName)?.ToLowerInvariant();
        var contentType = file.ContentType?.ToLowerInvariant();

        return contentType?.StartsWith("image/") == true ||
            new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp", ".tif", ".tiff" }.Contains(extension);
    }

    protected virtual async Task TouchStudentAsync(Student student)
    {
        var customer = await _workContext.GetCurrentCustomerAsync();
        student.UpdatedBy = customer?.Id ?? 0;
        student.UpdatedOnUtc = DateTime.UtcNow;
    }

    protected virtual string BuildStudentUsername(StudentModel model)
    {
        if (!string.IsNullOrWhiteSpace(model.Username))
            return model.Username.Trim();

        return model.GrNumber > 0
            ? $"student-{model.GrNumber}"
            : $"student-{Guid.NewGuid():N}";
    }

    protected virtual async Task<Customer> SaveStudentCustomerAsync(StudentModel model, Student student = null)
    {
        var customer = student?.CustomerId > 0
            ? await _customerService.GetCustomerByIdAsync(student.CustomerId)
            : null;

        var now = DateTime.UtcNow;
        var currentStore = await _storeContext.GetCurrentStoreAsync();

        if (customer == null)
        {
            customer = new Customer
            {
                CustomerGuid = Guid.NewGuid(),
                CreatedOnUtc = now,
                LastActivityDateUtc = now,
                RegisteredInStoreId = currentStore?.Id ?? 0,
                Active = model.IsActive
            };

            await _customerService.InsertCustomerAsync(customer);
        }

        customer.Username = BuildStudentUsername(model);
        customer.Email = model.Email?.Trim();
        customer.FirstName = model.FirstName;
        customer.LastName = model.LastName;
        customer.CNIC = model.CNIC;
        customer.DateOfBirth = model.DateOfBirth;
        customer.PhoneNumber = model.PhoneNumber;
        customer.Phone = model.PhoneNumber;
        customer.MontherTongue = model.MontherTongue;
        customer.Nationality = model.Nationality;
        customer.Religion = model.Religion;
        customer.BloodGroup = model.BloodGroup;
        customer.Caste = model.Caste;
        customer.Active = model.IsActive;
        customer.Deleted = model.Deleted;

        await _customerService.UpdateCustomerAsync(customer);

        var studentRole = await _customerService.GetCustomerRoleBySystemNameAsync(NopCustomerDefaults.StudentRoleName);
        if (studentRole != null)
        {
            var roleIds = await _customerService.GetCustomerRoleIdsAsync(customer, true);
            if (!roleIds.Contains(studentRole.Id))
                await _customerService.AddCustomerRoleMappingAsync(new CustomerCustomerRoleMapping { CustomerId = customer.Id, CustomerRoleId = studentRole.Id });
        }

        return customer;
    }

    protected virtual async Task ValidateStudentModelAsync(StudentModel model, Student existingStudent = null)
    {
        if (string.IsNullOrWhiteSpace(model.FirstName))
            ModelState.AddModelError(string.Empty, "First name is required.");

        if (string.IsNullOrWhiteSpace(model.LastName))
            ModelState.AddModelError(string.Empty, "Last name is required.");

        if (model.GrNumber <= 0)
            ModelState.AddModelError(string.Empty, "GR number is required.");

        var username = BuildStudentUsername(model);
        var existingCustomerByUsername = await _customerService.GetCustomerByUsernameAsync(username);
        if (existingCustomerByUsername != null && existingCustomerByUsername.Id != existingStudent?.CustomerId)
            ModelState.AddModelError(string.Empty, "A customer already exists with this username.");

        if (!string.IsNullOrWhiteSpace(model.Email))
        {
            var existingCustomerByEmail = await _customerService.GetCustomerByEmailAsync(model.Email.Trim());
            if (existingCustomerByEmail != null && existingCustomerByEmail.Id != existingStudent?.CustomerId)
                ModelState.AddModelError(string.Empty, "A customer already exists with this email.");
        }

        if (!string.IsNullOrWhiteSpace(model.CNIC))
        {
            var existingCustomerByCnic = await _customerService.GetCustomerByCnicAsync(model.CNIC.Trim());
            if (existingCustomerByCnic != null && existingCustomerByCnic.Id != existingStudent?.CustomerId)
                ModelState.AddModelError(string.Empty, "A customer already exists with this CNIC.");
        }
    }

    protected virtual bool IsStudentTemporaryPicturePath(string filePath)
    {
        return !string.IsNullOrWhiteSpace(filePath) &&
            filePath.Replace('\\', '/').StartsWith(StudentTemporaryPicturePathPrefix, StringComparison.InvariantCultureIgnoreCase);
    }

    protected virtual async Task DeleteTemporaryStudentPictureAsync(string filePath)
    {
        if (!IsStudentTemporaryPicturePath(filePath))
            return;

        await _attachmentFileService.DeleteAttachmentAsync(filePath);
    }

    protected virtual async Task RetainTemporaryStudentPictureAsync(StudentModel model, IFormFile studentPictureFile)
    {
        if (studentPictureFile == null || studentPictureFile.Length == 0)
        {
            if (IsStudentTemporaryPicturePath(model.TemporaryPicturePath))
                model.TemporaryPictureUrl = Url.Action("PreviewTemporaryStudentPicture", new { filePath = model.TemporaryPicturePath });

            return;
        }

        if (!IsAllowedStudentPicture(studentPictureFile))
            return;

        await DeleteTemporaryStudentPictureAsync(model.TemporaryPicturePath);

        var currentCustomer = await _workContext.GetCurrentCustomerAsync();
        var savedFile = await _attachmentFileService.SaveAttachmentAsync(
            studentPictureFile,
            StudentTemporaryPictureAttachmentModuleName,
            currentCustomer?.Id ?? 1,
            StudentPictureFileNamePrefix);

        model.TemporaryPicturePath = savedFile.FilePath;
        model.TemporaryPictureFileName = savedFile.FileName;
        model.TemporaryPictureUrl = Url.Action("PreviewTemporaryStudentPicture", new { filePath = savedFile.FilePath });
        model.RemovePicture = false;
    }

    protected virtual async Task<IFormFile> GetTemporaryStudentPictureFileAsync(StudentModel model)
    {
        if (!IsStudentTemporaryPicturePath(model.TemporaryPicturePath))
            return null;

        var physicalPath = _attachmentFileService.GetAttachmentPhysicalPath(model.TemporaryPicturePath);
        if (!System.IO.File.Exists(physicalPath))
            return null;

        var bytes = await System.IO.File.ReadAllBytesAsync(physicalPath);
        var memoryStream = new MemoryStream(bytes);
        var fileName = !string.IsNullOrWhiteSpace(model.TemporaryPictureFileName)
            ? model.TemporaryPictureFileName
            : Path.GetFileName(physicalPath);

        var contentTypeProvider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
        if (!contentTypeProvider.TryGetContentType(physicalPath, out var contentType))
            contentType = "application/octet-stream";

        return new FormFile(memoryStream, 0, memoryStream.Length, "StudentPictureFile", fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = contentType
        };
    }

    protected virtual async Task<byte[]> ReadStudentPictureBytesAsync(IFormFile studentPictureFile)
    {
        await using var stream = studentPictureFile.OpenReadStream();
        await using var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);

        return memoryStream.ToArray();
    }

    protected virtual IFormFile CreateBufferedStudentPictureFile(byte[] pictureBytes, IFormFile sourceFile)
    {
        var memoryStream = new MemoryStream(pictureBytes);

        return new FormFile(memoryStream, 0, memoryStream.Length, sourceFile.Name, sourceFile.FileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = sourceFile.ContentType
        };
    }

    protected virtual async Task DeleteStudentPictureAsync(Student student)
    {
        if (student?.PictureId <= 0)
            return;

        var picture = await _pictureService.GetPictureByIdAsync(student.PictureId);
        if (picture != null)
        {
            if (UseAttachmentStorageForStudentPictures && !string.IsNullOrWhiteSpace(picture.VirtualPath))
                await _attachmentFileService.DeleteAttachmentAsync(picture.VirtualPath);

            await _pictureService.DeletePictureAsync(picture);
        }

        student.PictureId = 0;
    }

    protected virtual async Task SaveStudentPictureAsync(Student student, StudentModel model, IFormFile studentPictureFile)
    {
        if (student == null)
            return;

        studentPictureFile ??= await GetTemporaryStudentPictureFileAsync(model);

        if (model.RemovePicture || studentPictureFile?.Length > 0)
            await DeleteStudentPictureAsync(student);

        if (studentPictureFile == null || studentPictureFile.Length == 0)
            return;

        if (!IsAllowedStudentPicture(studentPictureFile))
            throw new NopException("Only image files are allowed.");

        var pictureBytes = await ReadStudentPictureBytesAsync(studentPictureFile);
        Picture picture;
        if (UseAttachmentStorageForStudentPictures)
        {
            var attachmentFormFile = CreateBufferedStudentPictureFile(pictureBytes, studentPictureFile);
            var savedFile = await _attachmentFileService.SaveAttachmentAsync(
                attachmentFormFile,
                StudentPictureAttachmentModuleName,
                student.Id,
                StudentPictureFileNamePrefix);

            var pictureFormFile = CreateBufferedStudentPictureFile(pictureBytes, studentPictureFile);
            picture = await _pictureService.InsertPictureAsync(
                pictureFormFile,
                $"student-{student.Id}",
                savedFile.FilePath);
        }
        else
        {
            var pictureFormFile = CreateBufferedStudentPictureFile(pictureBytes, studentPictureFile);
            picture = await _pictureService.InsertPictureAsync(pictureFormFile, $"student-{student.Id}");
        }

        if (picture == null)
            throw new NopException("Wrong student picture format.");

        student.PictureId = picture.Id;

        await DeleteTemporaryStudentPictureAsync(model.TemporaryPicturePath);
        model.TemporaryPicturePath = null;
        model.TemporaryPictureFileName = null;
        model.TemporaryPictureUrl = null;
    }


    #endregion

    #region Students

    public virtual IActionResult Index()
    {
        return RedirectToAction("List");
    }

    [CheckPermission(StandardPermission.Students.MANAGE_STUDENTS)]
    public virtual async Task<IActionResult> List()
    {
        //prepare model
        var model = await _studentModelFactory.PrepareStudentSearchModelAsync(new StudentSearchModel());

        return View(model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Students.MANAGE_STUDENTS)]
    public virtual async Task<IActionResult> StudentList(StudentSearchModel searchModel)
    {
        //prepare model
        var model = await _studentModelFactory.PrepareStudentListModelAsync(searchModel);

        return Json(model);
    }

    [CheckPermission(StandardPermission.Students.MANAGE_STUDENTS)]
    public virtual async Task<IActionResult> Create()
    {
        //prepare model
        var model = await _studentModelFactory.PrepareStudentModelAsync(new StudentModel(), null);

        return View(model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    [CheckPermission(StandardPermission.Students.MANAGE_STUDENTS)]
    public virtual async Task<IActionResult> Create(StudentModel model, bool continueEditing, IFormFile studentPictureFile)
    {
        await ValidateStudentModelAsync(model);
        if (studentPictureFile?.Length > 0 && !IsAllowedStudentPicture(studentPictureFile))
            ModelState.AddModelError(string.Empty, "Only image files are allowed.");

        if (ModelState.IsValid)
        {
            var customer = await SaveStudentCustomerAsync(model);
            var student = model.ToEntity<Student>();
            student.CustomerId = customer.Id;
            student.CreatedBy = (await _workContext.GetCurrentCustomerAsync())?.Id ?? 0;
            student.CreatedOnUtc = DateTime.UtcNow;
            await TouchStudentAsync(student);
            await _studentService.InsertStudentAsync(student);
            await SaveStudentPictureAsync(student, model, studentPictureFile);
            await _studentService.UpdateStudentAsync(student);

            //activity log
            await _customerActivityService.InsertActivityAsync("AddNewStudent",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.AddNewStudent"), student.Id), student);



//{{StoreMappingSaveMethodCallHere}}

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Students.Students.Added"));

            if (!continueEditing)
                return RedirectToAction("List");

            return RedirectToAction("Edit", new { id = student.Id });
        }

        await RetainTemporaryStudentPictureAsync(model, studentPictureFile);

        //prepare model
        model = await _studentModelFactory.PrepareStudentModelAsync(model, null, true);

        //if we got this far, something failed, redisplay form
        return View(model);
    }

    [CheckPermission(StandardPermission.Students.MANAGE_STUDENTS)]
    public virtual async Task<IActionResult> Edit(int id)
    {
        //try to get a student with the specified id
        var student = await _studentService.GetStudentByIdAsync(id);
        if (student == null)
            return RedirectToAction("List");

        //prepare model
        var model = await _studentModelFactory.PrepareStudentModelAsync(null, student);

        return View(model);
    }

    [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
    [CheckPermission(StandardPermission.Students.MANAGE_STUDENTS)]
    public virtual async Task<IActionResult> Edit(StudentModel model, bool continueEditing, IFormFile studentPictureFile)
    {
        //try to get a student with the specified id
        var student = await _studentService.GetStudentByIdAsync(model.Id);
        if (student == null)
            return RedirectToAction("List");

        await ValidateStudentModelAsync(model, student);
        if (studentPictureFile?.Length > 0 && !IsAllowedStudentPicture(studentPictureFile))
            ModelState.AddModelError(string.Empty, "Only image files are allowed.");

        if (ModelState.IsValid)
        {
            var customer = await SaveStudentCustomerAsync(model, student);
            student = model.ToEntity(student);
            student.CustomerId = customer.Id;
            await SaveStudentPictureAsync(student, model, studentPictureFile);
            await TouchStudentAsync(student);
            await _studentService.UpdateStudentAsync(student);

            //activity log
            await _customerActivityService.InsertActivityAsync("EditStudent",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.EditStudent"), student.Id), student);



//{{StoreMappingSaveMethodCallHere}}

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Students.Students.Updated"));

            if (!continueEditing)
                return RedirectToAction("List");

            return RedirectToAction("Edit", new { id = student.Id });
        }

        await RetainTemporaryStudentPictureAsync(model, studentPictureFile);

        //prepare model
        model = await _studentModelFactory.PrepareStudentModelAsync(model, student, true);

        //if we got this far, something failed, redisplay form
        return View(model);
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Students.MANAGE_STUDENTS)]
    public virtual async Task<IActionResult> Delete(int id)
    {
        //try to get a student with the specified id
        var student = await _studentService.GetStudentByIdAsync(id);
        if (student == null)
            return RedirectToAction("List");

        try
        {
            await _studentService.DeleteStudentAsync(student);
            if (student.CustomerId > 0)
            {
                var customer = await _customerService.GetCustomerByIdAsync(student.CustomerId);
                if (customer != null)
                {
                    customer.Deleted = true;
                    customer.Active = false;
                    await _customerService.UpdateCustomerAsync(customer);
                }
            }

            //activity log
            await _customerActivityService.InsertActivityAsync("DeleteStudent",
                string.Format(await _localizationService.GetResourceAsync("ActivityLog.DeleteStudent"), student.Id), student);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Students.Students.Deleted"));

            return RedirectToAction("List");
        }
        catch (Exception exc)
        {
            await _notificationService.ErrorNotificationAsync(exc);
            return RedirectToAction("Edit", new { id = student.Id });
        }
    }

        [HttpPost]
    [CheckPermission(StandardPermission.Students.MANAGE_STUDENTS)]
    public virtual async Task<IActionResult> ActivateSelected(ICollection<int> selectedIds)
    {
        if (selectedIds == null || !selectedIds.Any())
            return NoContent();

        var students = await _studentService.GetStudentsByIdsAsync([.. selectedIds]);
        foreach (var student in students)
        {
            student.IsActive = true;
            await _studentService.UpdateStudentAsync(student);

            if (student.CustomerId > 0)
            {
                var customer = await _customerService.GetCustomerByIdAsync(student.CustomerId);
                if (customer != null)
                {
                    customer.Active = true;
                    await _customerService.UpdateCustomerAsync(customer);
                }
            }
        }

        return Json(new { Result = true });
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Students.MANAGE_STUDENTS)]
    public virtual async Task<IActionResult> DeactivateSelected(ICollection<int> selectedIds)
    {
        if (selectedIds == null || !selectedIds.Any())
            return NoContent();

        var students = await _studentService.GetStudentsByIdsAsync(selectedIds.ToArray());
        foreach (var student in students)
        {
            student.IsActive = false;
            await _studentService.UpdateStudentAsync(student);

            if (student.CustomerId > 0)
            {
                var customer = await _customerService.GetCustomerByIdAsync(student.CustomerId);
                if (customer != null)
                {
                    customer.Active = false;
                    await _customerService.UpdateCustomerAsync(customer);
                }
            }
        }

        return Json(new { Result = true });
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Students.MANAGE_STUDENTS)]
    public virtual async Task<IActionResult> StudentPicture(int studentId)
    {
        var student = await _studentService.GetStudentByIdAsync(studentId);
        if (student == null)
            return Json(new { success = false, message = "Student not found." });

        return Json(new
        {
            success = true,
            pictureId = student.PictureId,
            previewUrl = student.PictureId > 0 ? await _pictureService.GetPictureUrlAsync(student.PictureId, 260, false) : null
        });
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Students.MANAGE_STUDENTS)]
    public virtual async Task<IActionResult> UploadStudentPicture(int studentId, IFormFile file)
    {
        var student = await _studentService.GetStudentByIdAsync(studentId);
        if (student == null)
            return Json(new { success = false, message = "Student not found." });

        if (!IsAllowedStudentPicture(file))
            return Json(new { success = false, message = "Only image files are allowed." });

        if (student.PictureId > 0)
        {
            var existingPicture = await _pictureService.GetPictureByIdAsync(student.PictureId);
            if (existingPicture != null)
                await _pictureService.DeletePictureAsync(existingPicture);
        }

        var picture = await _pictureService.InsertPictureAsync(file, $"student-{student.Id}");
        student.PictureId = picture.Id;
        await TouchStudentAsync(student);
        await _studentService.UpdateStudentAsync(student);

        return Json(new
        {
            success = true,
            pictureId = picture.Id,
            previewUrl = await _pictureService.GetPictureUrlAsync(picture.Id, 260, false)
        });
    }

    [HttpPost]
    [CheckPermission(StandardPermission.Students.MANAGE_STUDENTS)]
    public virtual async Task<IActionResult> DeleteStudentPicture(int studentId)
    {
        var student = await _studentService.GetStudentByIdAsync(studentId);
        if (student == null)
            return Json(new { success = false, message = "Student not found." });

        if (student.PictureId > 0)
        {
            await DeleteStudentPictureAsync(student);
        }

        await TouchStudentAsync(student);
        await _studentService.UpdateStudentAsync(student);

        return Json(new { success = true });
    }

    [CheckPermission(StandardPermission.Students.MANAGE_STUDENTS)]
    public virtual IActionResult PreviewTemporaryStudentPicture(string filePath)
    {
        if (!IsStudentTemporaryPicturePath(filePath))
            return NotFound();

        var physicalPath = _attachmentFileService.GetAttachmentPhysicalPath(filePath);
        if (!System.IO.File.Exists(physicalPath))
            return NotFound();

        var contentTypeProvider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
        if (!contentTypeProvider.TryGetContentType(physicalPath, out var contentType))
            contentType = "application/octet-stream";

        return PhysicalFile(physicalPath, contentType);
    }

    #endregion
}
