using TKP.Server.Core.Entities;

namespace TKP.Server.Domain.Entites
{
    public sealed class LoginHistory : BaseEntity
    {
        public DateTime LoginTime { get; set; }
        public DateTime? LogoutTime { get; set; }
        public string? IpAddress { get; set; }
        public string? DeviceName { get; set; }
        public string? RefreshToken { get; set; }
        public string? DeviceType { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public required Guid UserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
    }
}
