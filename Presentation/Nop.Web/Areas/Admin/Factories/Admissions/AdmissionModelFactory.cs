using Nop.Core.Domain.Admissions;
using Nop.Core.Domain.GenericDropDowns;
using Nop.Services.Admissions;
using Nop.Services.GradeManagement;
using Nop.Services.Localization;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.Admissions;
using Nop.Web.Framework.Factories;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Web.Areas.Admin.Factories;

public partial class AdmissionModelFactory : IAdmissionModelFactory
{
    #region Fields

    protected readonly IAdmissionService _admissionService;
    protected readonly IGradeService _gradeService;
    protected readonly ILocalizationService _localizationService;
    protected readonly ILocalizedModelFactory _localizedModelFactory;
    protected readonly IStoreMappingSupportedModelFactory _storeMappingSupportedModelFactory;
    protected readonly IBaseAdminModelFactory _baseAdminModelFactory;

    #endregion

    #region Ctor

    public AdmissionModelFactory(IAdmissionService admissionService,
        IGradeService gradeService,
        ILocalizationService localizationService,
        ILocalizedModelFactory localizedModelFactory,
        IStoreMappingSupportedModelFactory storeMappingSupportedModelFactory,
        IBaseAdminModelFactory baseAdminModelFactory)
    {
        _admissionService = admissionService;
        _gradeService = gradeService;
        _localizationService = localizationService;
        _localizedModelFactory = localizedModelFactory;
        _storeMappingSupportedModelFactory = storeMappingSupportedModelFactory;
        _baseAdminModelFactory = baseAdminModelFactory;
    }

    #endregion

    #region Utilities

    protected virtual string GetSelectedText(IList<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> items, int selectedValue)
    {
        return items?.FirstOrDefault(item => item.Value == selectedValue.ToString())?.Text;
    }

    protected virtual async Task PrepareAdmissionDropDownsAsync(AdmissionModel model)
    {
        await _baseAdminModelFactory.PrepareStaticDropDownAsync(model.AvailableAdmissionStatuses, GenericDropdownEntity.AdmissionStatus);
        await _baseAdminModelFactory.PrepareStaticDropDownAsync(model.AvailablePreviousSchools, GenericDropdownEntity.PreviousSchoolName);
        await _baseAdminModelFactory.PrepareStaticDropDownAsync(model.AvailableBirthCities, GenericDropdownEntity.BirthCity);
        await _baseAdminModelFactory.PrepareStaticDropDownAsync(model.AvailableMotherTongues, GenericDropdownEntity.MotherTounge);
        await _baseAdminModelFactory.PrepareStaticDropDownAsync(model.AvailableNationalities, GenericDropdownEntity.Nationality);
        await _baseAdminModelFactory.PrepareStaticDropDownAsync(model.AvailableReligions, GenericDropdownEntity.Religion);
        await _baseAdminModelFactory.PrepareStaticDropDownAsync(model.AvailableBloodGroups, GenericDropdownEntity.BloodGroup);
        await _baseAdminModelFactory.PrepareStaticDropDownAsync(model.AvailableCastes, GenericDropdownEntity.Caste);
        await _baseAdminModelFactory.PrepareStaticDropDownAsync(model.AvailableGuardianTypes, GenericDropdownEntity.GuardianType);
        await _baseAdminModelFactory.PrepareStaticDropDownAsync(model.AvailableQualifications, GenericDropdownEntity.Qualification);
        await _baseAdminModelFactory.PrepareStaticDropDownAsync(model.AvailableProfessions, GenericDropdownEntity.Profession);

        model.AvailableGrades.Clear();
        model.AvailableGrades.Add(new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
        {
            Text = await _localizationService.GetResourceAsync("Admin.Common.Select"),
            Value = "0"
        });

        var grades = (await _gradeService.GetAllGradesAsync()).OrderBy(grade => grade.Name);
        foreach (var grade in grades)
        {
            model.AvailableGrades.Add(new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Text = grade.Name,
                Value = grade.Id.ToString(),
                Selected = grade.Id == model.GradeId
            });
        }
    }

    protected virtual void PrepareAdmissionDisplayNames(AdmissionModel model)
    {
        model.Status = GetSelectedText(model.AvailableAdmissionStatuses, model.StatusId);
        model.PreviousSchool = GetSelectedText(model.AvailablePreviousSchools, model.PreviousSchoolId);
        model.BirthCityName = GetSelectedText(model.AvailableBirthCities, model.BirthCity);
        model.MontherTongueName = GetSelectedText(model.AvailableMotherTongues, model.MontherTongue);
        model.NationalityName = GetSelectedText(model.AvailableNationalities, model.Nationality);
        model.ReligionName = GetSelectedText(model.AvailableReligions, model.Religion);
        model.BloodGroupName = GetSelectedText(model.AvailableBloodGroups, model.BloodGroup);
        model.CasteName = GetSelectedText(model.AvailableCastes, model.Caste);
        model.GuardianType = GetSelectedText(model.AvailableGuardianTypes, model.GuardianTypeId);
    }

    protected virtual async Task PrepareAdmissionDocumentTypesAsync(AdmissionGradeDocumentRequirementModel model)
    {
        await _baseAdminModelFactory.PrepareStaticDropDownAsync(model.AvailableAdmissionDocumentTypes, GenericDropdownEntity.AdmissionDocumentType, isMultiSelect: true);
    }

    #endregion

    #region Methods

    public virtual Task<AdmissionSearchModel> PrepareAdmissionSearchModelAsync(AdmissionSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        //prepare page parameters
        searchModel.SetGridPageSize();

        return Task.FromResult(searchModel);
    }

    public virtual async Task<AdmissionListModel> PrepareAdmissionListModelAsync(AdmissionSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        //get admissions
        var admissions = (await _admissionService.GetAllAdmissionsAsync()).ToPagedList(searchModel);

        //prepare list model
        var model = await new AdmissionListModel().PrepareToGridAsync(searchModel, admissions, () =>
        {
            //fill in model values from the entity
            return admissions.SelectAwait(async admission =>
            {
                var admissionModel = admission.ToModel<AdmissionModel>();
                await PrepareAdmissionDropDownsAsync(admissionModel);
                PrepareAdmissionDisplayNames(admissionModel);

                return admissionModel;
            });
        });

        return model;
    }

    public virtual async Task<AdmissionModel> PrepareAdmissionModelAsync(AdmissionModel model, Admission admission, bool excludeProperties = false)
    {
//{{LocalizedModelObject}}

        if (admission != null)
        {
            //fill in model values from the entity
            if (model == null)
            {
                model = admission.ToModel<AdmissionModel>();
            }


        }

        //set default values for the new model
        if (admission == null)
        {
            model.DateOfBirth = DateTime.UtcNow;
            model.FatherDateOfBirth = DateTime.UtcNow;
            model.MotherDateOfBirth = DateTime.UtcNow;
            model.GuardianDateOfBirth = DateTime.UtcNow;
            model.CreatedOnUtc = DateTime.UtcNow;
        }

        await PrepareAdmissionDropDownsAsync(model);
        PrepareAdmissionDisplayNames(model);

//{{Locales}}

//{{PrepareStoreCode}}

        return model;
    }

    public virtual async Task<AdmissionGradeDocumentRequirementListModel> PrepareAdmissionGradeDocumentRequirementListModelAsync(AdmissionGradeDocumentRequirementSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        var requirements = (await _admissionService.GetAllAdmissionGradeDocumentRequirementsAsync(gradeId: searchModel.GradeId)).ToPagedList(searchModel);

        var documentTypeLookupModel = new AdmissionGradeDocumentRequirementModel();
        await PrepareAdmissionDocumentTypesAsync(documentTypeLookupModel);

        var model = await new AdmissionGradeDocumentRequirementListModel().PrepareToGridAsync(searchModel, requirements, () =>
        {
            return requirements.SelectAwait(async requirement =>
            {
                var requirementModel = requirement.ToModel<AdmissionGradeDocumentRequirementModel>();
                requirementModel.AdmissionDocumentTypeName = GetSelectedText(documentTypeLookupModel.AvailableAdmissionDocumentTypes, requirement.AdmissionDocumentTypeId)
                    ?? requirement.AdmissionDocumentTypeId.ToString();

                return requirementModel;
            });
        });

        return model;
    }

    public virtual async Task<AdmissionGradeDocumentRequirementModel> PrepareAdmissionGradeDocumentRequirementModelAsync(
        AdmissionGradeDocumentRequirementModel model,
        AdmissionGradeDocumentRequirement admissionGradeDocumentRequirement,
        bool excludeProperties = false)
    {
        if (admissionGradeDocumentRequirement != null && model == null)
            model = admissionGradeDocumentRequirement.ToModel<AdmissionGradeDocumentRequirementModel>();

        model ??= new AdmissionGradeDocumentRequirementModel();

        await PrepareAdmissionDocumentTypesAsync(model);

        return model;
    }

    #endregion
}
