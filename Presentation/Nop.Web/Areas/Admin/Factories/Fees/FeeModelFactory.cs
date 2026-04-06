using Microsoft.Identity.Client;
using Nop.Core.Domain.Fees;
using Nop.Services.Customers;
using Nop.Services.Fees;
using Nop.Services.GenericDropDowns;
using Nop.Services.Localization;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.Fees;
using Nop.Web.Framework.Factories;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Web.Areas.Admin.Factories;

public partial class FeeModelFactory : IFeeModelFactory
{
    #region Fields

    protected readonly IFeeService _feeService;
    protected readonly ILocalizationService _localizationService;
    protected readonly ILocalizedModelFactory _localizedModelFactory;
    protected readonly IStoreMappingSupportedModelFactory _storeMappingSupportedModelFactory;
    protected readonly IBaseAdminModelFactory _baseAdminModelFactory;
    protected readonly ICustomerService _customerService;
    protected readonly IGenericDropDownOptionService _genericDropDownOptionService;

    #endregion

    #region Ctor

    public FeeModelFactory(IFeeService feeService,
        ILocalizationService localizationService,
        ILocalizedModelFactory localizedModelFactory,
        IStoreMappingSupportedModelFactory storeMappingSupportedModelFactory,
        IBaseAdminModelFactory baseAdminModelFactory,
        ICustomerService customerService,
        IGenericDropDownOptionService genericDropDownOptionService
        )
    {
        _feeService = feeService;
        _localizationService = localizationService;
        _localizedModelFactory = localizedModelFactory;
        _storeMappingSupportedModelFactory = storeMappingSupportedModelFactory;
        _baseAdminModelFactory = baseAdminModelFactory;
        _customerService = customerService;
        _genericDropDownOptionService = genericDropDownOptionService;
    }

    #endregion

    #region Utilities

    

    #endregion

    #region Methods

    public virtual Task<FeeSearchModel> PrepareFeeSearchModelAsync(FeeSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        //prepare page parameters
        searchModel.SetGridPageSize();

        return Task.FromResult(searchModel);
    }

    public virtual async Task<FeeListModel> PrepareFeeListModelAsync(FeeSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        //get fees
        var fees = (await _feeService.GetAllFeesAsync()).ToPagedList(searchModel);

        var students = fees != null && fees.Any() ? (await _customerService.GetAllCustomersAsync(ids: fees.Select(x => x.CustomerId))).ToList() : new List<Core.Domain.Customers.Customer>();
        var feeTypes = await _genericDropDownOptionService.GetGenericDropDownOptionsByEntityAsync(Core.Domain.GenericDropDowns.GenericDropdownEntity.FeesType);

        //prepare list model
        var model = await new FeeListModel().PrepareToGridAsync(searchModel, fees, () =>
        {
            //fill in model values from the entity
            return fees.SelectAwait(async fee =>
            {
                var feeModel = fee.ToModel<FeeModel>();

                var student = students.FirstOrDefault(x => x.Id == fee.CustomerId);
                if (student != null)
                    feeModel.StudentName = await _customerService.GetCustomerFullNameAsync(student);

                var feeType = feeTypes.FirstOrDefault(x => x.Id == fee.FeeTypeId);
                if (feeType != null)
                    feeModel.FeeType = await _localizationService.GetResourceAsync($"Admin.Option.{feeType.Text}");

                feeModel.FormattedFeeDate = fee.FeeDate.ToLocalTime().ToString("MMMM yyyy");

                return feeModel;
            });
        });

        return model;
    }

    public virtual async Task<FeeModel> PrepareFeeModelAsync(FeeModel model, Fee fee, bool excludeProperties = false)
    {
//{{LocalizedModelObject}}

        if (fee != null)
        {
            //fill in model values from the entity
            if (model == null)
            {
                model = fee.ToModel<FeeModel>();
            }
        }

        //set default values for the new model
        if (fee == null)
        {
            model.FeeDate = DateTime.UtcNow;
        }

        model.AvailableStudents.Add(new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
        {
            Text = await _localizationService.GetResourceAsync("Admin.Student.Select"),
            Value = "0"
        });

        var students = (await _customerService.GetAllCustomersAsync()).ToList();
        if (students != null && students.Any())
            foreach (var student in students)
                model.AvailableStudents.Add(new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Text = await _customerService.GetCustomerFullNameAsync(student),
                    Value = student.Id.ToString(),
                    Selected = student.Id == model.CustomerId
                });

        await _baseAdminModelFactory.PrepareStaticDropDownAsync(model.AvailableFeeTypes, Core.Domain.GenericDropDowns.GenericDropdownEntity.FeesType, selectedValue: model.FeeTypeId);

//{{Locales}}

//{{PrepareStoreCode}}

        return model;
    }

    #endregion
}