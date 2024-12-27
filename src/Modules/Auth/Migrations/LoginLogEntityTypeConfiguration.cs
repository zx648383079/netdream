using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Auth.Entities;
using NetDream.Modules.Auth.Repositories;

namespace NetDream.Modules.Auth.Migrations
{
    public class LoginLogEntityTypeConfiguration : IEntityTypeConfiguration<LoginLogEntity>
    {
        public void Configure(EntityTypeBuilder<LoginLogEntity> builder)
        {
            builder.ToTable("user_login_log", table => table.HasComment("账户登录日志表"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Ip).HasColumnName("ip").HasMaxLength(120);
            builder.Property(table => table.UserId).HasColumnName("user_id").HasDefaultValue(0);
            builder.Property(table => table.User).HasColumnName("user").HasMaxLength(100).HasComment("登陆账户");
            builder.Property(table => table.Status).HasColumnName("status").HasDefaultValue(0);
            builder.Property(table => table.Mode).HasColumnName("mode").HasMaxLength(20).HasDefaultValue(AuthRepository.LOGIN_MODE_WEB);
            builder.Property(table => table.PlatformId).HasColumnName("platform_id").HasDefaultValue(0).HasComment("平台id");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
