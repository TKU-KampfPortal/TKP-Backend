using Microsoft.AspNetCore.Http;

namespace TKP.Server.Application.HelperServices.Cookie
{
    public interface ICookieService
    {
        void SetKey(string key, string value, int? expireDays = null, bool httpOnly = true, bool secure = true, SameSiteMode sameSite = SameSiteMode.Strict);
        void SetKey<T>(string key, T value, int? expireDays = null, bool httpOnly = true, bool secure = true, SameSiteMode sameSite = SameSiteMode.Strict);
        string? GetKey(string key);
        T? GetKey<T>(string key);
        void DeleteKey(string key);
    }
}
