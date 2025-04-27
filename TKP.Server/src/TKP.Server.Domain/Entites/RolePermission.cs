using TKP.Server.Core.Entities;

namespace TKP.Server.Domain.Entites
{
    public sealed class RolePermission : BaseEntity
    {
        public Guid RoleId { get; set; }
        public ApplicationRole Role { get; set; } = null!;
        public string Permission { get; set; }
    }
}
