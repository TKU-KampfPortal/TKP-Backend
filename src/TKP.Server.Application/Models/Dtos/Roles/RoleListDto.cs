namespace TKP.Server.Application.Models.Dtos.Roles
{
    public class RoleListDto : BaseResponseDto
    {
        public string Name { get; set; } = string.Empty;
        public string NormalizedName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
