using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Chat.Entities;

namespace NetDream.Modules.Chat.Migrations
{
    public class MessageEntityTypeConfiguration : IEntityTypeConfiguration<MessageEntity>
    {
        public void Configure(EntityTypeBuilder<MessageEntity> builder)
        {
            builder.ToTable("chat_message", table => table.HasComment(""));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Type).HasColumnName("type").HasMaxLength(2).HasDefaultValue(0);
            builder.Property(table => table.Content).HasColumnName("content").HasMaxLength(400).HasComment("����");
            builder.Property(table => table.ExtraRule).HasColumnName("extra_rule").HasMaxLength(400)
                .HasDefaultValue(string.Empty).HasComment("�����滻����");
            builder.Property(table => table.ItemId).HasColumnName("item_id")
                .HasDefaultValue(0).HasComment("����id");
            builder.Property(table => table.ReceiveId).HasColumnName("receive_id")
                .HasDefaultValue(0).HasComment("�����û�");
            builder.Property(table => table.GroupId).HasColumnName("group_id")
                .HasDefaultValue(0).HasComment("����Ⱥ");
            builder.Property(table => table.UserId).HasColumnName("user_id").HasComment("�����û�");
            builder.Property(table => table.Status).HasColumnName("status").HasMaxLength(2).HasDefaultValue(0);
            builder.Property(table => table.DeletedAt).HasColumnName("deleted_at");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
