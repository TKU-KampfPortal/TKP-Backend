using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TKP.Server.Domain.Entites;

namespace TKP.Server.Infrastructure.Data.Config
{
    public class LoginHistoryConfig : IEntityTypeConfiguration<LoginHistory>
    {
        public void Configure(EntityTypeBuilder<LoginHistory> builder)
        {
            builder.ToTable("LoginHistories");

            builder.HasKey(x => x.Id);

            builder.HasOne(p => p.ApplicationUser)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.Property(x => x.UserId).IsRequired();

            builder.Property(x => x.LoginTime).IsRequired();

            builder.Property(x => x.IpAddress).IsRequired().HasMaxLength(45);
        }
    }

}
