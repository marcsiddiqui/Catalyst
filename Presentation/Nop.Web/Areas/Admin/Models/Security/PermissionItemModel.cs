using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Security;

/// <summary>
/// Represents a permission item model
/// </summary>
public partial record PermissionItemModel : BaseNopEntityModel
{
    #region Ctor

    public PermissionItemModel()
    {
        SelectedCustomerRoleIds = new List<int>();
        AvailableCustomerRoles = new List<SelectListItem>();
    }

    #endregion

    #region Properties

    public string PermissionName { get; set; }

    public string PermissionAppliedFor { get; set; }

    [NopResourceDisplayName("Admin.Customers.CustomerRoles")]
    public IList<int> SelectedCustomerRoleIds { get; set; }

    public IList<SelectListItem> AvailableCustomerRoles { get; set; }

    public bool IsEnabledOnInstall { get; set; }

    #endregion
}

public partial record PermissionItemGroupModel : BaseNopEntityModel
{
    #region Ctor

    public PermissionItemGroupModel()
    {
        PermissionItems = new List<PermissionItemModel>();
    }

    #endregion

    #region Properties

    public string CategoryName { get; set; }

    public IList<PermissionItemModel> PermissionItems { get; set; }

    #endregion
}