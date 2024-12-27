using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Chat.Entities;

namespace NetDream.Modules.Chat.Migrations
{
    public class HistoryEntityTypeConfiguration : IEntityTypeConfiguration<HistoryEntity>
    {
        public void Configure(EntityTypeBuilder<HistoryEntity> builder)
        {
            builder.ToTable("chat_history", table => table.HasComment(""));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.ItemType).HasColumnName("item_type").HasMaxLength(2);
            builder.Property(table => table.ItemId).HasColumnName("item_id").HasComment("������ʷ");
            builder.Property(table => table.UserId).HasColumnName("user_id").HasComment("�����û�");
            builder.Property(table => table.UnreadCount).HasColumnName("unread_count").HasComment("δ����Ϣ����");
            builder.Property(table => table.LastMessage).HasColumnName("last_message").HasComment("���һ����Ϣ");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
