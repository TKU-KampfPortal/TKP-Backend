using TKP.Server.Application.Configurations.Queries;
using TKP.Server.Application.Enum;
using TKP.Server.Application.Exceptions;
using TKP.Server.Application.HelperServices;
using TKP.Server.Application.HelperServices.Cache;
using TKP.Server.Application.HelperServices.Cookie;
using TKP.Server.Application.Repositories.Interface;
using TKP.Server.Domain.Enums;

namespace TKP.Server.Application.Features.Auth.Queries.Logout
{
    public sealed class LogoutQueryHandler : BaseQueryHandler<LogoutQuery, bool>
    {
        private readonly ICookieService _cookieService;
        private readonly ILoginHistoryRepository _loginHistoryRepository;
        private readonly IClaimService _claimService;
        private readonly ICacheService<bool> _accessTokenCacheService;
        private readonly ITokenService _tokenService;
        public LogoutQueryHandler(
            ILoginHistoryRepository loginHistoryRepository,
            ICookieService cookieService,
            IClaimService claimService,
            ICacheFactory cacheFactory,
            ITokenService tokenService)
        {
            _tokenService = tokenService;
            _loginHistoryRepository = loginHistoryRepository;
            _cookieService = cookieService;
            _claimService = claimService;
            _accessTokenCacheService = cacheFactory.GetCacheService<bool>();
        }
        protected override async Task<bool> HandleAsync(LogoutQuery request, CancellationToken cancellationToken = default)
        {
            var accessToken = _tokenService.GetCurrentAccessToken();
            var loginHistoryId = _claimService.GetLoginHistoryId();
            var expiredDate = _claimService.GetExpiredTime();
            var tokenId = _claimService.GetTokenId();
            var loginHistory = await _loginHistoryRepository.GetByIdAsync(Guid.Parse(loginHistoryId));


            if (loginHistory is null)
            {
                throw new NotFoundException("Login history not found");
            }

            loginHistory.RefreshToken = null;
            loginHistory.RefreshTokenExpiryTime = null;
            loginHistory.LogoutTime = DateTime.UtcNow;

            _cookieService.DeleteKey(CookieKeyEnum.RefreshToken.ToString());
            await _accessTokenCacheService.SetValueAsync(PrefixCacheKey.AccessToken, tokenId, false, expiredDate);
            await _loginHistoryRepository.UpdateAsync(loginHistory);

            return true;
        }
    }
}
