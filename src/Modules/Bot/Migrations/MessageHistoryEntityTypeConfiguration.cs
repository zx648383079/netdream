using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Bot.Entities;

namespace NetDream.Modules.Bot.Migrations
{
    public class MessageHistoryEntityTypeConfiguration : IEntityTypeConfiguration<MessageHistoryEntity>
    {
        public void Configure(EntityTypeBuilder<MessageHistoryEntity> builder)
        {
            builder.ToTable("bot_message_history", table => table.HasComment(""));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.BotId).HasColumnName("bot_id").HasComment("����΢�Ź��ں�ID");
            builder.Property(table => table.Type).HasColumnName("type").HasMaxLength(1).HasDefaultValue(0).HasComment("��Ϣ����");
            builder.Property(table => table.ItemType).HasColumnName("item_type").HasMaxLength(1).HasDefaultValue(0).HasComment("��������");
            builder.Property(table => table.ItemId).HasColumnName("item_id").HasDefaultValue(0).HasComment("��Ӧ����ID");
            builder.Property(table => table.From).HasColumnName("from").HasMaxLength(50).HasComment("�����û�ID");
            builder.Property(table => table.To).HasColumnName("to").HasMaxLength(50).HasComment("��Ӧ�û�ID");
            builder.Property(table => table.Content).HasColumnName("content").HasComment("��Ϣ������");
            builder.Property(table => table.IsMark).HasColumnName("is_mark").HasDefaultValue(0).HasComment("�Ƿ���");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
