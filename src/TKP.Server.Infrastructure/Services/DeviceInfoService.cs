using Microsoft.AspNetCore.Http;
using TKP.Server.Application.HelperServices;
using UAParser;

namespace TKP.Server.Infrastructure.Services
{
    public class DeviceInfoService : IDeviceInfoService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Parser _parser;
        private const string Unknown = "Unknown";
        public DeviceInfoService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _parser = Parser.GetDefault();
        }

        public string GetIpAddress()
        {
            var headers = _httpContextAccessor.HttpContext?.Request.Headers;
            if (headers != null && headers.ContainsKey("X-Forwarded-For"))
            {
                // Get First IP (IP of client)
                return headers["X-Forwarded-For"].ToString().Split(',').FirstOrDefault()?.Trim() ?? Unknown;
            }

            return _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? Unknown;
        }


        public string GetUserAgent() =>
            _httpContextAccessor.HttpContext?.Request.Headers["User-Agent"].ToString() ?? Unknown;

        public string GetDeviceType()
        {
            var userAgent = GetUserAgent().ToLower();
            if (userAgent.Contains("mobile")) return "Mobile";
            if (userAgent.Contains("tablet")) return "Tablet";
            if (userAgent.Contains("windows") || userAgent.Contains("macintosh")) return "Desktop";
            return Unknown;
        }

        public string GetDeviceName()
        {
            var userAgent = GetUserAgent();
            if (userAgent == Unknown) return Unknown;
            var clientInfo = _parser.Parse(userAgent);
            return $"{clientInfo.Device.Family} ({clientInfo.OS})";
        }
    }

}
