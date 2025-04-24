using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TKP.Server.Application.Exceptions;
using TKP.Server.Application.HelperServices;
using TKP.Server.Domain.Entites;
using TKP.Server.Infrastructure.ConfigSetting;

namespace TKP.Server.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtConfigSetting _jwtConfigSetting;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TokenService(JwtConfigSetting jWTConfigSetting
            , IHttpContextAccessor httpContextAccessor)
        {
            _jwtConfigSetting = jWTConfigSetting;
            _httpContextAccessor = httpContextAccessor;
        }
        public (string token, int validDays) GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumber);
            return (Convert.ToBase64String(randomNumber), _jwtConfigSetting.RefreshTokenValidityInDays);
        }

        public string GenerateAccessToken(ApplicationUser user, IEnumerable<string> roles, LoginHistory loginHistory)
        {
            var key = Encoding.ASCII.GetBytes(_jwtConfigSetting.SecretKey);
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(JwtConfigSetting.JwtTokenId, Guid.NewGuid().ToString()),
            new Claim(JwtConfigSetting.LoginHistoryIdClaimType, loginHistory.Id.ToString()),
            };
            // Thêm các claim cho từng role của user
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _jwtConfigSetting?.Issuer,
                Audience = _jwtConfigSetting?.Audience,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(_jwtConfigSetting?.TokenValidityInHours ?? 1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public async Task<ClaimsIdentity> GetPrincipalFromExpiredToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidIssuer = _jwtConfigSetting.Issuer,
                ValidAudience = _jwtConfigSetting.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfigSetting.SecretKey)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var validateResult = await tokenHandler.ValidateTokenAsync(token, tokenValidationParameters);

            if (!validateResult.IsValid)
            {
                throw new InvalidModelException("Invalid access token or refresh token");
            }

            var securityToken = validateResult.SecurityToken;

            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return validateResult.ClaimsIdentity;

        }

        public string GetCurrentAccessToken()
        {
            var tokenString = _httpContextAccessor?.HttpContext?.Request.Headers["Authorization"]
            .ToString()
            .Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase)
            .Trim();

            if (string.IsNullOrWhiteSpace(tokenString))
            {
                throw new InvalidModelException("Invalid access token");
            }

            return tokenString;
        }
    }
}
