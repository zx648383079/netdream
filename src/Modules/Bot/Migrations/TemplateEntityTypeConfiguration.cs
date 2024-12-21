using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Bot.Entities;

namespace NetDream.Modules.Bot.Migrations
{
    public class TemplateEntityTypeConfiguration : IEntityTypeConfiguration<TemplateEntity>
    {
        public void Configure(EntityTypeBuilder<TemplateEntity> builder)
        {
            builder.ToTable("bot_template", table => table.HasComment("΢��ģ����Ϣģ��"));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.BotId).HasColumnName("bot_id").HasComment("����΢�Ź��ں�ID");
            builder.Property(table => table.TemplateId).HasColumnName("template_id").HasMaxLength(64).HasComment("ģ��id");
            builder.Property(table => table.Title).HasColumnName("title").HasMaxLength(100).HasComment("����");
            builder.Property(table => table.Content).HasColumnName("content").HasComment("����");
            builder.Property(table => table.Example).HasColumnName("example").HasDefaultValue(string.Empty).HasComment("ʾ��");
        }
    }
}
