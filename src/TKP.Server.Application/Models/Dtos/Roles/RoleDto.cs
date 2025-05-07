using TKP.Server.Domain.Permissions;

namespace TKP.Server.Application.Models.Dtos.Roles
{
    public sealed class RoleDto : BaseResponseDto
    {
        public string Name { get; set; } = string.Empty;
        public string NormalizedName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<Permission> Permissions { get; set; } = new();
    }
}
