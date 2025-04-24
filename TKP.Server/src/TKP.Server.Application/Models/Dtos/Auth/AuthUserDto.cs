namespace TKP.Server.Application.Models.Dtos.Auth
{
    public sealed class AuthUserDto : BaseResponseDto
    {
        public string Username { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
        public List<string> Permissions { get; set; } = new List<string>();

    }
}
