
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Framework.Models;

/// <summary>
/// Represents base nopCommerce entity model
/// </summary>
public partial record BaseNopEntityModel : BaseNopModel
{
    /// <summary>
    /// Gets or sets model identifier
    /// </summary>
    public virtual int Id { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Holidays.Fields.CreatedBy")]
    public virtual int CreatedBy { get; set; }
    
    [NopResourceDisplayName("Admin.Configuration.Holidays.Fields.CreatedBy")]
    public virtual string CreatedByName { get; set; }

    [NopResourceDisplayName("AdmiNamen.Configuration.Holidays.Fields.CreatedOnUtc")]
    public virtual DateTime CreatedOnUtc { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Holidays.Fields.UpdatedBy")]
    public virtual int UpdatedBy { get; set; }
    
    [NopResourceDisplayName("Admin.Configuration.Holidays.Fields.UpdatedBy")]
    public virtual string UpdatedByName { get; set; }

    [NopResourceDisplayName("Admin.Configuration.Holidays.Fields.UpdatedOnUtc")]
    public virtual DateTime? UpdatedOnUtc { get; set; }
}