namespace TKP.Server.Application.HelperServices
{
    /// <summary>
    /// Provides methods to retrieve information about the device and client making the HTTP request.
    /// </summary>
    public interface IDeviceInfoService
    {
        /// <summary>
        /// Gets the IP address of the client making the request.
        /// </summary>
        /// <returns>The client's IP address as a string.</returns>
        string GetIpAddress();

        /// <summary>
        /// Gets the raw User-Agent string from the client's request headers.
        /// </summary>
        /// <returns>The User-Agent string.</returns>
        string GetUserAgent();

        /// <summary>
        /// Determines the device type (e.g., Desktop, Mobile, Tablet) based on the User-Agent.
        /// </summary>
        /// <returns>A string representing the device type.</returns>
        string GetDeviceType();

        /// <summary>
        /// Gets a descriptive name of the device or browser based on the User-Agent.
        /// </summary>
        /// <returns>The name of the device or browser.</returns>
        string GetDeviceName();
    }

}
