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
            builder.Property(table => table.BotId).HasColumnName("bot_id").HasComment("所属微信公众号ID");
            builder.Property(table => table.Type).HasColumnName("type").HasMaxLength(1).HasDefaultValue(0).HasComment("消息类型");
            builder.Property(table => table.ItemType).HasColumnName("item_type").HasMaxLength(1).HasDefaultValue(0).HasComment("发送类型");
            builder.Property(table => table.ItemId).HasColumnName("item_id").HasDefaultValue(0).HasComment("相应规则ID");
            builder.Property(table => table.From).HasColumnName("from").HasMaxLength(50).HasComment("请求用户ID");
            builder.Property(table => table.To).HasColumnName("to").HasMaxLength(50).HasComment("相应用户ID");
            builder.Property(table => table.Content).HasColumnName("content").HasComment("消息体内容");
            builder.Property(table => table.IsMark).HasColumnName("is_mark").HasDefaultValue(0).HasComment("是否标记");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
