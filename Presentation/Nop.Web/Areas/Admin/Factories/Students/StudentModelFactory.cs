using Nop.Core.Domain.Students;
using Nop.Core.Domain.GenericDropDowns;
using Nop.Services.Customers;
using Nop.Services.Media;
using Nop.Services.Students;
using Nop.Services.Localization;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.Students;
using Nop.Web.Framework.Factories;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Web.Areas.Admin.Factories;

public partial class StudentModelFactory : IStudentModelFactory
{
    #region Fields

    protected readonly IStudentService _studentService;
    protected readonly ICustomerService _customerService;
    protected readonly IPictureService _pictureService;
    protected readonly IBaseAdminModelFactory _baseAdminModelFactory;
    protected readonly ILocalizationService _localizationService;
    protected readonly ILocalizedModelFactory _localizedModelFactory;
    protected readonly IStoreMappingSupportedModelFactory _storeMappingSupportedModelFactory;

    #endregion

    #region Ctor

    public StudentModelFactory(IStudentService studentService,
        ICustomerService customerService,
        IPictureService pictureService,
        IBaseAdminModelFactory baseAdminModelFactory,
        ILocalizationService localizationService,
        ILocalizedModelFactory localizedModelFactory,
        IStoreMappingSupportedModelFactory storeMappingSupportedModelFactory)
    {
        _studentService = studentService;
        _customerService = customerService;
        _pictureService = pictureService;
        _baseAdminModelFactory = baseAdminModelFactory;
        _localizationService = localizationService;
        _localizedModelFactory = localizedModelFactory;
        _storeMappingSupportedModelFactory = storeMappingSupportedModelFactory;
    }

    #endregion

    #region Utilities
    protected virtual string GetSelectedText(IList<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> items, int selectedValue)
    {
        return items?.FirstOrDefault(item => item.Value == selectedValue.ToString())?.Text;
    }

    protected virtual async Task PrepareStudentDropDownsAsync(StudentModel model)
    {
        await _baseAdminModelFactory.PrepareStaticDropDownAsync(model.AvailablePreviousSchools, GenericDropdownEntity.PreviousSchoolName);
        await _baseAdminModelFactory.PrepareStaticDropDownAsync(model.AvailableBirthCities, GenericDropdownEntity.BirthCity);
        await _baseAdminModelFactory.PrepareStaticDropDownAsync(model.AvailableMotherTongues, GenericDropdownEntity.MotherTounge);
        await _baseAdminModelFactory.PrepareStaticDropDownAsync(model.AvailableNationalities, GenericDropdownEntity.Nationality);
        await _baseAdminModelFactory.PrepareStaticDropDownAsync(model.AvailableReligions, GenericDropdownEntity.Religion);
        await _baseAdminModelFactory.PrepareStaticDropDownAsync(model.AvailableBloodGroups, GenericDropdownEntity.BloodGroup);
        await _baseAdminModelFactory.PrepareStaticDropDownAsync(model.AvailableCastes, GenericDropdownEntity.Caste);
        await _baseAdminModelFactory.PrepareStaticDropDownAsync(model.AvailableGuardianTypes, GenericDropdownEntity.GuardianType);
    }

    protected virtual void PrepareStudentDisplayNames(StudentModel model)
    {
        model.FullName = string.Join(" ", new[] { model.FirstName, model.MiddleName, model.LastName }.Where(value => !string.IsNullOrWhiteSpace(value)));
        model.PreviousSchool = GetSelectedText(model.AvailablePreviousSchools, model.PreviousSchoolId);
        model.BirthCityName = GetSelectedText(model.AvailableBirthCities, model.BirthCity);
        model.MontherTongueName = GetSelectedText(model.AvailableMotherTongues, model.MontherTongue);
        model.NationalityName = GetSelectedText(model.AvailableNationalities, model.Nationality);
        model.ReligionName = GetSelectedText(model.AvailableReligions, model.Religion);
        model.BloodGroupName = GetSelectedText(model.AvailableBloodGroups, model.BloodGroup);
        model.CasteName = GetSelectedText(model.AvailableCastes, model.Caste);
        model.GuardianType = GetSelectedText(model.AvailableGuardianTypes, model.GuardianTypeId);
    }

    protected virtual async Task ApplyCustomerFieldsAsync(StudentModel model, Student student)
    {
        if (student?.CustomerId <= 0)
            return;

        var customer = await _customerService.GetCustomerByIdAsync(student.CustomerId);
        if (customer == null)
            return;

        model.Username = customer.Username;
        model.Email = customer.Email;
        model.FirstName = customer.FirstName;
        model.LastName = customer.LastName;
        model.CNIC = customer.CNIC;
        model.DateOfBirth = customer.DateOfBirth;
        model.PhoneNumber = customer.PhoneNumber;
        model.MontherTongue = customer.MontherTongue;
        model.Nationality = customer.Nationality;
        model.Religion = customer.Religion;
        model.BloodGroup = customer.BloodGroup;
        model.Caste = customer.Caste;
    }


    #endregion

    #region Methods

    public virtual Task<StudentSearchModel> PrepareStudentSearchModelAsync(StudentSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        //prepare page parameters
        searchModel.SetGridPageSize();

        return Task.FromResult(searchModel);
    }

    public virtual async Task<StudentListModel> PrepareStudentListModelAsync(StudentSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        //get students
        var students = (await _studentService.GetAllStudentsAsync()).ToPagedList(searchModel);

        //prepare list model
        var model = await new StudentListModel().PrepareToGridAsync(searchModel, students, () =>
        {
            //fill in model values from the entity
            return students.SelectAwait(async student =>
            {
                var studentModel = student.ToModel<StudentModel>();
                await ApplyCustomerFieldsAsync(studentModel, student);
                await PrepareStudentDropDownsAsync(studentModel);
                PrepareStudentDisplayNames(studentModel);

                return studentModel;
            });
        });

        return model;
    }

    public virtual async Task<StudentModel> PrepareStudentModelAsync(StudentModel model, Student student, bool excludeProperties = false)
    {
//{{LocalizedModelObject}}

        if (student != null)
        {
            //fill in model values from the entity
            if (model == null)
            {
                model = student.ToModel<StudentModel>();
            }

            await ApplyCustomerFieldsAsync(model, student);
        }

        //set default values for the new model
        if (student == null)
        {
            model.IsActive = true;
            model.DateOfBirth = DateTime.UtcNow;
            model.CreatedOnUtc = DateTime.UtcNow;
        }

        await PrepareStudentDropDownsAsync(model);
        PrepareStudentDisplayNames(model);

        model.PictureUrl = model.PictureId > 0
            ? await _pictureService.GetPictureUrlAsync(model.PictureId, 180, false)
            : null;

//{{Locales}}

//{{PrepareStoreCode}}

        return model;
    }

    #endregion
}
