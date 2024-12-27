using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.OpenPlatform.Entities;

namespace NetDream.Modules.OpenPlatform.Migrations
{
    public class UserTokenEntityTypeConfiguration : IEntityTypeConfiguration<UserTokenEntity>
    {
        public void Configure(EntityTypeBuilder<UserTokenEntity> builder)
        {
            builder.ToTable("open_user_token", table => table.HasComment("�û���Ȩƽ̨����"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.PlatformId).HasColumnName("platform_id");
            builder.Property(table => table.Token).HasColumnName("token");
            builder.Property(table => table.IsSelf).HasColumnName("is_self").HasDefaultValue(0).HasComment("�Ƿ�ʱ�û���̨��ӵ�");
            builder.Property(table => table.ExpiredAt).HasColumnName("expired_at").HasComment("����ʱ��");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
