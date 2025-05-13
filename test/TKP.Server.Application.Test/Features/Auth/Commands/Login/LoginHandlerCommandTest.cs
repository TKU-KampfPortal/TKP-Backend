using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using TKP.Server.Application.Exceptions;
using TKP.Server.Application.Features.Auth.Commands.Login;
using TKP.Server.Application.HelperServices;
using TKP.Server.Application.HelperServices.Cookie;
using TKP.Server.Application.OptionSetting;
using TKP.Server.Application.Repositories.Interface;
using TKP.Server.Domain.Entites;
using Xunit;

namespace TKP.Server.Application.Test.Features.Auth.Commands.Login
{
    public class LoginHandlerCommandTest
    {
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly Mock<RoleManager<ApplicationRole>> _roleManagerMock;
        private readonly Mock<ILoginHistoryRepository> _loginHistoryRepositoryMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly Mock<ICookieService> _cookieServiceMock;
        private readonly Mock<IDeviceInfoService> _deviceInfoServiceMock;
        private readonly Mock<ILogger<LoginCommandHandler>> _loggerMock;
        private readonly LoginCommandHandler _handler;
        public LoginHandlerCommandTest()
        {
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(
                new Mock<IUserStore<ApplicationUser>>().Object,
                null, null, null, null, null, null, null, null);

            _roleManagerMock = new Mock<RoleManager<ApplicationRole>>(
                new Mock<IRoleStore<ApplicationRole>>().Object,
                null, null, null, null);

            _loginHistoryRepositoryMock = new Mock<ILoginHistoryRepository>();
            _tokenServiceMock = new Mock<ITokenService>();
            _cookieServiceMock = new Mock<ICookieService>();
            _deviceInfoServiceMock = new Mock<IDeviceInfoService>();
            _loggerMock = new Mock<ILogger<LoginCommandHandler>>();

            _handler = new LoginCommandHandler(
                _userManagerMock.Object,
                _loginHistoryRepositoryMock.Object,
                _tokenServiceMock.Object,
                _cookieServiceMock.Object,
                _deviceInfoServiceMock.Object,
                new AuthConfigSetting(5),
                _loggerMock.Object,
                _roleManagerMock.Object
            );
        }


        [Fact]
        public async Task LoginCommandHandler_ShouldThrowUnauthorizeException_WhenUserNotFound()
        {
            // Arrange
            var command = new LoginCommand
            {
                Body = new BodyLoginCommand
                {
                    UserName = "nonexistentuser",
                    Password = "password123"
                }
            };

            // Mock UserManager to return null for FindByNameAsync
            _userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser)null);
            _userManagerMock.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser)null);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizeException>(async () => await _handler.Handle(command, CancellationToken.None));
        }
        [Fact]
        public async Task LoginCommandHandler_ShouldThrowUnauthorizeException_WhenAccountIsLocked()
        {
            // Arrange
            var command = new LoginCommand
            {
                Body = new BodyLoginCommand
                {
                    UserName = "lockeduser",
                    Password = "password123"
                }
            };


            // Mock UserManager to return a user with a lockout end date in the future
            var lockedUser = new ApplicationUser
            {
                DisplayName = "userDisplayName",
                LockoutEnd = DateTimeOffset.UtcNow.AddMinutes(10)
            };

            _userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(lockedUser);
            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizeException>(async () => await _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task LoginCommandHandler_ShouldThrowUnauthorizeException_WhenMaxFailedLoginAttemptsExceeded()
        {
            // Arrange
            var command = new LoginCommand
            {
                Body = new BodyLoginCommand
                {
                    UserName = "user",
                    Password = "password123"
                }
            };

            // Mock UserManager to return a user with failed login attempts
            var user = new ApplicationUser
            {
                DisplayName = "userDisplayName",
                LockoutEnd = null
            };

            _userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(user);
            _userManagerMock.Setup(x => x.GetAccessFailedCountAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(2);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizeException>(async () => await _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task LoginCommandHandler_ShouldThrowUnauthorizeException_WhenPasswordIsInvalid()
        {
            // Arrange
            var command = new LoginCommand
            {
                Body = new BodyLoginCommand
                {
                    UserName = "user",
                    Password = "wrongpassword"
                }
            };


            // Mock UserManager to return a user and check password to return false
            var user = new ApplicationUser
            {
                DisplayName = "userDisplayName",
                LockoutEnd = null
            };
            _userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(user);
            _userManagerMock.Setup(x => x.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizeException>(async () => await _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task LoginCommandHandler_ShouldGetTheAccessToken_WhenValidUsernamePassword()
        {
            // Arrange
            var command = new LoginCommand
            {
                Body = new BodyLoginCommand
                {
                    UserName = "user",
                    Password = "password123"
                }
            };


            // Mock UserManager to return a user and check password to return true
            var user = new ApplicationUser
            {
                DisplayName = "userDisplayName",
                LockoutEnd = null
            };
            _userManagerMock.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(user);
            _userManagerMock.Setup(x => x.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(true);

            // Mock RoleManger to return a role
            _userManagerMock.Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(new List<string> { "Admin" });


            var roles = new List<ApplicationRole>
            {
                new ApplicationRole
                {
                    Id = Guid.NewGuid(),
                    Name = "Admin"
                }
            }.AsQueryable();

            var asyncRoles = new TestAsyncEnumerable<ApplicationRole>(roles);

            _roleManagerMock.Setup(x => x.Roles).Returns(asyncRoles);

            _cookieServiceMock.Setup(x => x.SetKey(
                It.IsAny<string>(), // key
                It.IsAny<string>(), // value
                It.IsAny<int?>(),   // expireDays
                It.IsAny<bool>(),   // httpOnly
                It.IsAny<bool>(),   // secure
                It.IsAny<SameSiteMode>() // sameSite
            ));

            _loginHistoryRepositoryMock.Setup(x => x.AddAsync(It.IsAny<LoginHistory>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Mock TokenService to return a valid token
            var token = "valid_token";
            var validDays = 5;

            _tokenServiceMock.Setup(x => x.GenerateAccessToken(It.IsAny<ApplicationUser>(), It.IsAny<IEnumerable<string>>(), It.IsAny<LoginHistory>()))
                             .Returns(token);
            _tokenServiceMock.Setup(x => x.GenerateRefreshToken()).Returns((token, validDays));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            _cookieServiceMock.Verify(x => x.SetKey(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<int?>(),
                It.IsAny<bool>(),
                It.IsAny<bool>(),
                It.IsAny<SameSiteMode>()
            ), Times.Once);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(token, result.AccessToken);
        }
    }
}
