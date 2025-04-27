using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TKP.Server.Application.Configurations.Commands;
using TKP.Server.Application.Enum;
using TKP.Server.Application.Exceptions;
using TKP.Server.Application.HelperServices;
using TKP.Server.Application.HelperServices.Cookie;
using TKP.Server.Application.Models.Dtos.Auth;
using TKP.Server.Application.OptionSetting;
using TKP.Server.Application.Repositories.Interface;
using TKP.Server.Domain.Entites;

namespace TKP.Server.Application.Features.Auth.Commands.Login
{
    public class LoginCommandHandler : BaseCommandHandler<LoginCommand, AuthTokenResponseDto>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        private readonly ILogger<LoginCommandHandler> _logger;
        private readonly ILoginHistoryRepository _loginHistoryRepository;
        private readonly ITokenService _tokenService;
        private readonly ICookieService _cookieService;
        private readonly IDeviceInfoService _deviceInfoService;
        private readonly AuthConfigSetting _authConfigSetting;
        public LoginCommandHandler(UserManager<ApplicationUser> userManager
            , ILoginHistoryRepository loginHistoryRepository
            , ITokenService tokenService
            , ICookieService cookieService
            , IDeviceInfoService deviceInfoService
            , AuthConfigSetting authConfigSetting
            , ILogger<LoginCommandHandler> logger
            , RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _loginHistoryRepository = loginHistoryRepository;
            _tokenService = tokenService;
            _cookieService = cookieService;
            _deviceInfoService = deviceInfoService;
            _authConfigSetting = authConfigSetting;
            _logger = logger;
            _roleManager = roleManager;
        }
        protected override async Task<AuthTokenResponseDto> HandleAsync(LoginCommand request, CancellationToken cancellationToken)
        {
            // Try to find the user by username
            var user = await _userManager.FindByNameAsync(request.Body.UserName);

            if (user is null)
                // If not found, try to find the user by email
                user = await _userManager.FindByEmailAsync(request.Body.UserName);

            if (user is null)
                // If the user is still not found, throw an unauthorized exception
                throw new UnauthorizeException("Invalid username or email");

            // Check if the account is locked (if lockout end date is set and still in the future)
            if (user.LockoutEnd != null && user.LockoutEnd > DateTime.UtcNow)
            {
                throw new UnauthorizeException($"Account is locked until {user.LockoutEnd}");
            }

            // Get the number of failed login attempts
            int failedLoginAttempts = await _userManager.GetAccessFailedCountAsync(user);

            // If the user has exceeded the max number of failed login attempts
            if (failedLoginAttempts >= _authConfigSetting.MaxFailedLoginAttempts)
            {
                // Lock the account for 15 minutes (you can change the duration here)
                var lockoutEndDate = DateTimeOffset.UtcNow.AddMinutes(15);
                await _userManager.SetLockoutEndDateAsync(user, lockoutEndDate);
                throw new UnauthorizeException("Account is locked due to too many failed login attempts");
            }

            // Check if the provided password is correct
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Body.Password);

            if (!isPasswordValid)
            {
                // Increment the failed login attempts count
                await _userManager.AccessFailedAsync(user);

                // Log the failed login attempt (optional but helpful for debugging)
                _logger.LogWarning($"Failed login attempt for user {request.Body.UserName} from IP: {_deviceInfoService.GetIpAddress()}");

                // If password is incorrect, throw an exception
                throw new UnauthorizeException("Invalid password");
            }

            // Reset the failed login attempts count after a successful login
            await _userManager.ResetAccessFailedCountAsync(user);

            // Get the roles of the user (e.g., admin, user, etc.)
            var roleNames = await _userManager.GetRolesAsync(user);

            var roleIds = await _roleManager.Roles.Where(Roles => roleNames.ToList().Contains(Roles.Name))
                .Select(Roles => Roles.Id.ToString())
                .ToListAsync(cancellationToken);

            // Create a new login history record
            var loginHistory = new LoginHistory
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                LoginTime = DateTime.UtcNow,
                IpAddress = _deviceInfoService.GetIpAddress(),
                DeviceName = _deviceInfoService.GetDeviceName(),
                DeviceType = _deviceInfoService.GetDeviceType(),
            };

            // Generate a refresh token and access token
            var refreshToken = GenerateRefreshToken(loginHistory);
            var accessToken = _tokenService.GenerateAccessToken(user, roleIds, loginHistory);

            // Save the refresh token to the cookies (for later use)
            _cookieService.SetKey(CookieKeyEnum.RefreshToken.ToString(), refreshToken);

            // Save the login history to the repository/database
            await _loginHistoryRepository.AddAsync(loginHistory, cancellationToken);

            // Return the access token as the response
            var response = new AuthTokenResponseDto() { AccessToken = accessToken };

            return response;
        }

        private string GenerateRefreshToken(LoginHistory loginHistory)
        {
            var (refreshToken, validDays) = _tokenService.GenerateRefreshToken();
            loginHistory.RefreshToken = refreshToken;
            loginHistory.RefreshTokenExpiryTime = DateTime.UtcNow.Add(TimeSpan.FromDays(validDays));
            return refreshToken;
        }
    }
}
