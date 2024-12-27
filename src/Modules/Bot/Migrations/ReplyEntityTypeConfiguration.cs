using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Bot.Entities;

namespace NetDream.Modules.Bot.Migrations
{
    public class ReplyEntityTypeConfiguration : IEntityTypeConfiguration<ReplyEntity>
    {
        public void Configure(EntityTypeBuilder<ReplyEntity> builder)
        {
            builder.ToTable("bot_reply", table => table.HasComment("微信回复"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.BotId).HasColumnName("bot_id").HasComment("所属微信公众号ID");
            builder.Property(table => table.Event).HasColumnName("event").HasMaxLength(20).HasComment("事件");
            builder.Property(table => table.Keywords).HasColumnName("keywords").HasMaxLength(60).HasDefaultValue(string.Empty).HasComment("关键词");
            builder.Property(table => table.Match).HasColumnName("match").HasDefaultValue(0).HasComment("关键词匹配模式");
            builder.Property(table => table.Content).HasColumnName("content").HasComment("微信返回数据");
            builder.Property(table => table.Type).HasColumnName("type").HasMaxLength(1).HasDefaultValue(0).HasComment("素材类型");
            builder.Property(table => table.Status).HasColumnName("status").HasDefaultValue(1).HasComment("激活");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
