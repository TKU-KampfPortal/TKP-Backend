namespace TKP.Server.Application.Features.Roles.Shared.Dtos
{
    public sealed record RoleRequestBody
    {
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public List<string> PermissionKeys { get; init; } = new();
    }
}
