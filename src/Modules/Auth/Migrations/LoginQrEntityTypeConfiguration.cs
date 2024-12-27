using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Auth.Entities;

namespace NetDream.Modules.Auth.Migrations
{
    public class LoginQrEntityTypeConfiguration : IEntityTypeConfiguration<LoginQrEntity>
    {
        public void Configure(EntityTypeBuilder<LoginQrEntity> builder)
        {
            builder.ToTable("LoginQr", table => table.HasComment(""));
            builder.HasKey(i => i.Id);
        }
    }
}
