using Microsoft.AspNetCore.Identity;

namespace TKP.Server.Domain.Entites
{
    public sealed class ApplicationRole : IdentityRole<Guid>
    {
        public string Description { get; set; } = string.Empty;
    }
}
