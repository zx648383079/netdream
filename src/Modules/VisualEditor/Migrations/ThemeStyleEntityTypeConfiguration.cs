using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.VisualEditor.Entities;

namespace NetDream.Modules.VisualEditor.Migrations;
public class ThemeStyleEntityTypeConfiguration : IEntityTypeConfiguration<ThemeStyleEntity>
{
    public void Configure(EntityTypeBuilder<ThemeStyleEntity> builder)
    {
        builder.ToTable("tpl_theme_style", table => table.HasComment("页面和组件提供的样式"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.ComponentId).HasColumnName("component_id").HasMaxLength(10);
        builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(30);
        builder.Property(table => table.Description).HasColumnName("description").HasMaxLength(200);
        builder.Property(table => table.Thumb).HasColumnName("thumb").HasMaxLength(100);
        builder.Property(table => table.Path).HasColumnName("path").HasMaxLength(200);
        
    }
}