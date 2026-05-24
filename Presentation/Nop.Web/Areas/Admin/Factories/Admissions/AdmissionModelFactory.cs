using Nop.Core.Domain.Admissions;
using Nop.Services.Admissions;
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
    protected readonly ILocalizationService _localizationService;
    protected readonly ILocalizedModelFactory _localizedModelFactory;
    protected readonly IStoreMappingSupportedModelFactory _storeMappingSupportedModelFactory;

    #endregion

    #region Ctor

    public AdmissionModelFactory(IAdmissionService admissionService,
        ILocalizationService localizationService,
        ILocalizedModelFactory localizedModelFactory,
        IStoreMappingSupportedModelFactory storeMappingSupportedModelFactory)
    {
        _admissionService = admissionService;
        _localizationService = localizationService;
        _localizedModelFactory = localizedModelFactory;
        _storeMappingSupportedModelFactory = storeMappingSupportedModelFactory;
    }

    #endregion

    #region Utilities

    

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
            
        }

//{{Locales}}

//{{PrepareStoreCode}}

        return model;
    }

    #endregion
}