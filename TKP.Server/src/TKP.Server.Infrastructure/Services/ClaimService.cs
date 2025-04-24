using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TKP.Server.Application.HelperServices;
using TKP.Server.Infrastructure.ConfigSetting;

namespace TKP.Server.Infrastructure.Services
{
    public class ClaimService : IClaimService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClaimService(IHttpContextAccessor httpContextAccessor) => this._httpContextAccessor = httpContextAccessor;

        public string GetUserId(ClaimsIdentity? claimsIdentity = null)
        {
            if (claimsIdentity is null)
            {
                return this.GetClaim(ClaimTypes.NameIdentifier);
            }

            return claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        }

        public string GetUserName() => this.GetClaim(ClaimTypes.Name);
        public string GetLoginHistoryId(ClaimsIdentity? claimsIdentity = null)
        {
            if (claimsIdentity is null)
            {
                return this.GetClaim(JwtConfigSetting.LoginHistoryIdClaimType);
            }

            return claimsIdentity.FindFirst(JwtConfigSetting.LoginHistoryIdClaimType)?.Value ?? string.Empty;
        }
        public string GetTokenId(ClaimsIdentity? claimsIdentity = null)
        {
            if (claimsIdentity is null)
            {
                return this.GetClaim(JwtConfigSetting.JwtTokenId);
            }

            return claimsIdentity.FindFirst(JwtConfigSetting.JwtTokenId)?.Value ?? string.Empty;
        }
        // Get the expiration time (issued at and expiration time)
        public DateTime GetExpiredTime(ClaimsIdentity? claimsIdentity = null)
        {
            var expirationClaim = claimsIdentity?.FindFirst("exp") ??
                                  _httpContextAccessor.HttpContext?.User?.FindFirst("exp");

            if (expirationClaim != null && long.TryParse(expirationClaim.Value, out var seconds))
            {
                return DateTimeOffset.FromUnixTimeSeconds(seconds).UtcDateTime;
            }

            // Trả về DateTime.MinValue nếu không có hoặc không parse được exp
            return DateTime.MinValue;
        }

        private string GetClaim(string key) => this._httpContextAccessor.HttpContext?.User?.FindFirst(key)?.Value ?? string.Empty;
    }
}
