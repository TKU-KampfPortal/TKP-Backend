using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TKP.Server.Domain.Entites
{
    public sealed class ApplicationUser : IdentityUser<Guid>
    {
        [MaxLength(255)]
        public string? AvatarUrl { get; set; }

        [MaxLength(255)]
        public required string DisplayName { get; set; }

        public bool IsActive { get; set; } = true;

        public int FailedLoginAttempts { get; set; } = 0;
    }
}
