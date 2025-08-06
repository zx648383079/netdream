using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Bot.Entities;

namespace NetDream.Modules.Bot.Migrations
{
    public class EditorTemplateCategoryEntityTypeConfiguration : IEntityTypeConfiguration<EditorCategoryEntity>
    {
        public void Configure(EntityTypeBuilder<EditorCategoryEntity> builder)
        {
            builder.ToTable("bot_editor_template_category", table => table.HasComment("微信图文模板分类"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(20).HasComment("模板标题");
            builder.Property(table => table.ParentId).HasColumnName("parent_id").HasDefaultValue(0);
        }
    }
}
