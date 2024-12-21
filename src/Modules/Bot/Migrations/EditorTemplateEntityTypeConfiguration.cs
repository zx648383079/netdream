using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Bot.Entities;

namespace NetDream.Modules.Bot.Migrations
{
    public class EditorTemplateEntityTypeConfiguration : IEntityTypeConfiguration<EditorTemplateEntity>
    {
        public void Configure(EntityTypeBuilder<EditorTemplateEntity> builder)
        {
            builder.ToTable("bot_editor_template", table => table.HasComment("微信图文模板"));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Type).HasColumnName("type").HasMaxLength(2).HasDefaultValue(0).HasComment("类型：素材、节日、行业");
            builder.Property(table => table.CatId).HasColumnName("cat_id").HasDefaultValue(0).HasComment("详细分类");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(100).HasComment("模板标题");
            builder.Property(table => table.Content).HasColumnName("content").HasComment("模板内容");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
