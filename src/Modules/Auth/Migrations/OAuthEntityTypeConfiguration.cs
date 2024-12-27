using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Auth.Entities;
using NetDream.Modules.Auth.Repositories;

namespace NetDream.Modules.Auth.Migrations
{
    public class OAuthEntityTypeConfiguration : IEntityTypeConfiguration<OauthEntity>
    {
        public void Configure(EntityTypeBuilder<OauthEntity> builder)
        {
            builder.ToTable("user_oauth");
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.PlatformId).HasColumnName("platform_id").HasDefaultValue(0).HasComment("平台id");
            builder.Property(table => table.Nickname).HasColumnName("nickname").HasMaxLength(30).HasDefaultValue(string.Empty).HasComment("昵称");
            builder.Property(table => table.Vendor).HasColumnName("vendor").HasMaxLength(20).HasDefaultValue(AuthRepository.OAUTH_TYPE_QQ);
            builder.Property(table => table.Unionid).HasColumnName("unionid").HasMaxLength(100).HasDefaultValue(string.Empty).HasComment("联合id");
            builder.Property(table => table.Identity).HasColumnName("identity").HasMaxLength(100);
            builder.Property(table => table.Data).HasColumnName("data");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
