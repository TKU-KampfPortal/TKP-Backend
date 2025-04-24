using Microsoft.AspNetCore.Identity;
using TKP.Server.Application.Configurations.Commands;
using TKP.Server.Application.Enum;
using TKP.Server.Application.Exceptions;
using TKP.Server.Application.HelperServices;
using TKP.Server.Application.HelperServices.Cache;
using TKP.Server.Application.HelperServices.Cookie;
using TKP.Server.Application.Models.Dtos.Auth;
using TKP.Server.Application.Repositories.Interface;
using TKP.Server.Domain.Entites;
using TKP.Server.Domain.Enums;

namespace TKP.Server.Application.Features.Auth.Commands.RefreshToken
{
    public sealed class RefreshTokenCommandHandler : BaseCommandHandler<RefreshTokenCommand, AuthTokenResponseDto>
    {
        private readonly IClaimService _claimService;
        private readonly ITokenService _tokenService;
        private readonly ILoginHistoryRepository _loginHistoryRepository;
        private readonly ICookieService _cookieService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICacheService<bool> _accessTokenCacheService;
        public RefreshTokenCommandHandler(IClaimService claimService
            , ITokenService tokenService
            , ILoginHistoryRepository loginHistoryRepository
            , ICookieService cookieService
            , UserManager<ApplicationUser> userManager
            , ICacheFactory cacheFactory)
        {
            _claimService = claimService;
            _tokenService = tokenService;
            _loginHistoryRepository = loginHistoryRepository;
            _cookieService = cookieService;
            _userManager = userManager;
            _accessTokenCacheService = cacheFactory.GetCacheService<bool>();
        }

        protected override async Task<AuthTokenResponseDto> HandleAsync(RefreshTokenCommand request, CancellationToken cancellationToken = default)
        {
            var claimIdentity = await _tokenService.GetPrincipalFromExpiredToken(request.Body.AccessToken)
                ?? throw new UnauthorizeException("Access token is not valid");
            var loginHistoryIdAsString = _claimService.GetLoginHistoryId(claimIdentity);

            if (string.IsNullOrWhiteSpace(loginHistoryIdAsString))
            {
                throw new UnauthorizeException("Access Token is not valid");
            }

            var userId = _claimService.GetUserId(claimIdentity);
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                throw new NotFoundException("User not found");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var loginHistoryId = new Guid(loginHistoryIdAsString);
            var loginHistory = await _loginHistoryRepository.GetByIdAsync(loginHistoryId);
            var refreshToken = _cookieService.GetKey(CookieKeyEnum.RefreshToken.ToString());

            if (loginHistory is null || refreshToken != loginHistory.RefreshToken || loginHistory.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                throw new UnauthorizeException("Access Token is not valid");
            }
            var newAccessToken = _tokenService.GenerateAccessToken(user, roles, loginHistory);

            var expiredAccessTokenDate = _claimService.GetExpiredTime(claimIdentity);
            var expiredTimeSpan = expiredAccessTokenDate - DateTime.UtcNow;

            // Set the expired access token to cache
            if (expiredTimeSpan > TimeSpan.Zero)
            {
                var expireTokenId = _claimService.GetTokenId(claimIdentity);
                await _accessTokenCacheService.SetValueAsync(PrefixCacheKey.AccessToken, expireTokenId, true, expiredTimeSpan);
            }

            // Update login history 
            await _loginHistoryRepository.UpdateAsync(loginHistory);

            return new AuthTokenResponseDto()
            {
                AccessToken = newAccessToken,
            };

        }
    }

}
