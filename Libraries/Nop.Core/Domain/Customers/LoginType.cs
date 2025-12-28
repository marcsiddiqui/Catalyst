namespace Nop.Core.Domain.Customers
{
    /// <summary>
    /// Represents the customer registration type formatting enumeration
    /// </summary>
    public enum LoginType
    {
        /// <summary>
        /// Logged in from Website
        /// </summary>
        Website = 0,

        /// <summary>
        /// Logged in from App
        /// </summary>
        MobileApp = 1,

        /// <summary>
        /// Logged in from API
        /// </summary>
        PublicApi = 2,

        /// <summary>
        /// Logged in from App using Google SSO
        /// </summary>
        GoogleSSO = 3,

        /// <summary>
        /// Logged in from App using Apple SSO
        /// </summary>
        AppleSSO = 4,

        /// <summary>
        /// Logged in from website using Google SSO
        /// </summary>
        GoogleWebsiteSSO = 5,

        /// <summary>
        /// Logged in from website using Apple SSO
        /// </summary>
        AppleWebsiteSSO = 6
    }
}