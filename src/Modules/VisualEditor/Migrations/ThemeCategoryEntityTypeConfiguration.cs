using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.VisualEditor.Entities;

namespace NetDream.Modules.VisualEditor.Migrations;
public class ThemeCategoryEntityTypeConfiguration : IEntityTypeConfiguration<ThemeCategoryEntity>
{
    public void Configure(EntityTypeBuilder<ThemeCategoryEntity> builder)
    {
        builder.ToTable("tpl_theme_category", table => table.HasComment("主题市场分类"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(20);
        builder.Property(table => table.ParentId).HasColumnName("parent_id").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.Description).HasColumnName("description").HasMaxLength(255);
        builder.Property(table => table.Thumb).HasColumnName("thumb").HasMaxLength(100);
        
    }
}