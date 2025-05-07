using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TKP.Server.Domain.Entites;

namespace TKP.Server.Infrastructure.Data.Config
{
    public sealed class RolePermissionConfig : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            builder.ToTable("RolePermissions");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Permission).IsRequired();

            builder.Property(p => p.RoleId).IsRequired();
        }
    }
}
