using System.Security.Claims;
using TKP.Server.Domain.Entites;

namespace TKP.Server.Application.HelperServices
{
    public interface ITokenService
    {
        /// <summary>
        /// Generates a secure refresh token.
        /// </summary>
        /// <returns>A tuple containing the token and its validity in days.</returns>
        (string token, int validDays) GenerateRefreshToken();

        /// <summary>
        /// Extracts claims from an expired access token.
        /// </summary>
        /// <param name="token">The expired access token.</param>
        /// <returns>A ClaimsIdentity representing the user.</returns>
        Task<ClaimsIdentity> GetPrincipalFromExpiredToken(string? token);

        /// <summary>
        /// Generates a new access token (JWT) for the user.
        /// </summary>
        /// <param name="user">The application user.</param>
        /// <param name="roles">List of roles assigned to the user.</param>
        /// <param name="loginHistory">User login history.</param>
        /// <returns>A signed JWT string.</returns>
        string GenerateAccessToken(ApplicationUser user, IEnumerable<string> roles, LoginHistory loginHistory);
        /// <summary>
        /// Get the current access token
        /// </summary>
        /// <returns>Access token in context</returns>
        string GetCurrentAccessToken();
    }
}
