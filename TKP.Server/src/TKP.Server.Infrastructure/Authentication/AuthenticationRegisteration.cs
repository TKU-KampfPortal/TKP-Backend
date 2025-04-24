using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using TKP.Server.Application.HelperServices.Cache;
using TKP.Server.Domain.Enums;
using TKP.Server.Infrastructure.ConfigSetting;

namespace TKP.Server.Infrastructure.Authentication
{
    public static class AuthenticationRegisteration
    {
        public static WebApplicationBuilder AddAuthenticationServices(this WebApplicationBuilder builder)
        {
            var configuration = builder.Configuration;
            var services = builder.Services;

            var jwtSettings = configuration.GetSection(nameof(JwtConfigSetting)).Get<JwtConfigSetting>();
            services.AddSingleton<JwtConfigSetting>(jwtSettings);

            var key = Encoding.ASCII.GetBytes(jwtSettings.SecretKey);

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),

                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings.Issuer,

                    ValidateAudience = true,
                    ValidAudience = jwtSettings.Audience,

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(2)
                };
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async context =>
                    {
                        var tokenString = context.Request.Headers["Authorization"]
                            .ToString()
                            .Replace("Bearer ", "", StringComparison.OrdinalIgnoreCase)
                            .Trim();

                        var handler = new JwtSecurityTokenHandler();
                        var jwtToken = handler.ReadJwtToken(tokenString);

                        // Get Token Id
                        var tokenId = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtConfigSetting.JwtTokenId)?.Value;
                        if (string.IsNullOrEmpty(tokenId))
                        {
                            context.Fail("Token is invalid.");
                            return;
                        }
                        var tokenCacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService<bool>>();

                        var isBlacklisted = await tokenCacheService.ExistsKeyAsync(PrefixCacheKey.AccessToken, tokenId);

                        if (isBlacklisted)
                        {
                            context.Fail("Token is blacklisted.");
                        }
                    }
                };
            });

            return builder;
        }

    }
}
