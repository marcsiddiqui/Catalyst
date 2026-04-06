using Nop.Core.Caching;

namespace Nop.Services.GenericDropDowns;

/// <summary>
/// Represents default values related to customer services
/// </summary>
public static partial class NopGenericDropdownDefaults
{
    
    #region Caching defaults

    /// <summary>
    /// Gets a key for caching
    /// </summary>
    /// <remarks>
    /// {0} : system name
    /// </remarks>
    public static CacheKey GenericDropdownByEntity => new("Nop.genericdropdown.byentity.{0}");

    /// <summary>
    /// Gets a key for caching
    /// </summary>
    public static CacheKey GenericDropdownDefault => new("Nop.genericdropdown.");

    #endregion
}