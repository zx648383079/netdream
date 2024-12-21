using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.OpenPlatform.Entities;

namespace NetDream.Modules.OpenPlatform.Migrations
{
    public class PlatformEntityTypeConfiguration : IEntityTypeConfiguration<PlatformEntity>
    {
        public void Configure(EntityTypeBuilder<PlatformEntity> builder)
        {
            builder.ToTable("open_platform", table => table.HasComment("��������Ȩ��Ϣ"));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(20);
            builder.Property(table => table.Description).HasColumnName("description").HasDefaultValue(string.Empty).HasComment("˵��");
            builder.Property(table => table.Type).HasColumnName("type").HasMaxLength(1).HasDefaultValue(0);
            builder.Property(table => table.Domain).HasColumnName("domain").HasMaxLength(50);
            builder.HasIndex("appid").IsUnique();
            builder.Property(table => table.Appid).HasColumnName("appid").HasMaxLength(12);
            builder.Property(table => table.Secret).HasColumnName("secret").HasMaxLength(32);
            builder.Property(table => table.SignType).HasColumnName("sign_type").HasMaxLength(1).HasDefaultValue(0).HasComment("ǩ����ʽ");
            builder.Property(table => table.SignKey).HasColumnName("sign_key").HasMaxLength(32).HasDefaultValue(string.Empty).HasComment("ǩ����Կ");
            builder.Property(table => table.EncryptType).HasColumnName("encrypt_type").HasMaxLength(1).HasDefaultValue(0).HasComment("���ܷ�ʽ");
            builder.Property(table => table.PublicKey).HasColumnName("public_key").HasComment("��Կ");
            builder.Property(table => table.Rules).HasColumnName("rules").HasDefaultValue(string.Empty).HasComment("������ʵ���ַ");
            builder.Property(table => table.Status).HasColumnName("status").HasMaxLength(1).HasDefaultValue(0);
            builder.Property(table => table.AllowSelf).HasColumnName("allow_self").HasDefaultValue(0).HasComment("�Ƿ������̨�û��Լ����");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
