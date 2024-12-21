using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Bot.Entities;

namespace NetDream.Modules.Bot.Migrations
{
    public class TemplateEntityTypeConfiguration : IEntityTypeConfiguration<TemplateEntity>
    {
        public void Configure(EntityTypeBuilder<TemplateEntity> builder)
        {
            builder.ToTable("bot_template", table => table.HasComment("微信模板消息模板"));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.BotId).HasColumnName("bot_id").HasComment("所属微信公众号ID");
            builder.Property(table => table.TemplateId).HasColumnName("template_id").HasMaxLength(64).HasComment("模板id");
            builder.Property(table => table.Title).HasColumnName("title").HasMaxLength(100).HasComment("标题");
            builder.Property(table => table.Content).HasColumnName("content").HasComment("内容");
            builder.Property(table => table.Example).HasColumnName("example").HasDefaultValue(string.Empty).HasComment("示例");
        }
    }
}
