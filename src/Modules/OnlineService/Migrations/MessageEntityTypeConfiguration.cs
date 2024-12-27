using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.OnlineService.Entities;

namespace NetDream.Modules.OnlineService.Migrations
{
    public class MessageEntityTypeConfiguration : IEntityTypeConfiguration<MessageEntity>
    {
        public void Configure(EntityTypeBuilder<MessageEntity> builder)
        {
            builder.ToTable("service_message", table => table.HasComment(""));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.UserId).HasColumnName("user_id").HasDefaultValue(0).HasComment("������");
            builder.Property(table => table.SessionId).HasColumnName("session_id");
            builder.Property(table => table.SendType).HasColumnName("send_type").HasDefaultValue(0).HasComment("�����ߵ���ݣ�0��ѯ��1�ͷ�");
            builder.Property(table => table.Type).HasColumnName("type").HasMaxLength(2).HasDefaultValue(0).HasComment("��������");
            builder.Property(table => table.Content).HasColumnName("content").HasDefaultValue(string.Empty);
            builder.Property(table => table.ExtraRule).HasColumnName("extra_rule").HasMaxLength(400)
                .HasDefaultValue(string.Empty).HasComment("�����滻����");
            builder.Property(table => table.Status).HasColumnName("status").HasMaxLength(2).HasDefaultValue(0);
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
