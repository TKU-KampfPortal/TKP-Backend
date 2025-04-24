using System.Security.Claims;

namespace TKP.Server.Application.HelperServices
{
    public interface IClaimService
    {
        //TODO: Implement the claim service
        string GetUserId(ClaimsIdentity? claimsIdentity = null);
        string GetTokenId(ClaimsIdentity? claimsIdentity = null);
        string GetUserName();
        string GetLoginHistoryId(ClaimsIdentity? claimsIdentity = null);
        DateTime GetExpiredTime(ClaimsIdentity? claimsIdentity = null);
    }
}
