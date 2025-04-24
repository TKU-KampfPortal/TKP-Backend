using Microsoft.AspNetCore.Http;
using System.Text.Json;
using TKP.Server.Application.HelperServices.Cookie;

namespace TKP.Server.Infrastructure.Services
{
    public class CookieService : ICookieService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CookieService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void SetKey(string key, string value, int? expireDays = null, bool httpOnly = true, bool secure = true, SameSiteMode sameSite = SameSiteMode.Strict)
        {
            var options = new CookieOptions
            {
                HttpOnly = httpOnly,
                Secure = secure,
                SameSite = sameSite,
                Expires = expireDays.HasValue ? DateTimeOffset.UtcNow.AddDays(expireDays.Value) : null
            };

            _httpContextAccessor.HttpContext?.Response.Cookies.Append(key, value, options);
        }
        public void SetKey<T>(string key, T value, int? expireDays = null, bool httpOnly = true, bool secure = true, SameSiteMode sameSite = SameSiteMode.Strict)
        {
            var options = new CookieOptions
            {
                HttpOnly = httpOnly,
                Secure = secure,
                SameSite = sameSite,
                Expires = expireDays.HasValue ? DateTimeOffset.UtcNow.AddDays(expireDays.Value) : null
            };

            string jsonValue = JsonSerializer.Serialize(value);

            _httpContextAccessor.HttpContext?.Response.Cookies.Append(key, jsonValue, options);
        }
        public string? GetKey(string key)
        {
            string? value = null;
            _httpContextAccessor.HttpContext?.Request.Cookies.TryGetValue(key, out value);
            return value;
        }

        public T? GetKey<T>(string key)
        {
            var json = GetKey(key);
            if (string.IsNullOrWhiteSpace(json)) return default;

            try
            {
                return JsonSerializer.Deserialize<T>(json);
            }
            catch
            {
                return default;
            }
        }

        public void DeleteKey(string key)
        {
            _httpContextAccessor.HttpContext?.Response.Cookies.Delete(key);
        }
    }
}
