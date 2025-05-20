using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.VisualEditor.Entities;

namespace NetDream.Modules.VisualEditor.Migrations;
public class SiteWeightEntityTypeConfiguration : IEntityTypeConfiguration<SiteWeightEntity>
{
    public void Configure(EntityTypeBuilder<SiteWeightEntity> builder)
    {
        builder.ToTable("tpl_site_weight", table => table.HasComment("站点的所有自定义组件及设置"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.SiteId).HasColumnName("site_id").HasMaxLength(10);
        builder.Property(table => table.ComponentId).HasColumnName("component_id").HasMaxLength(10);
        builder.Property(table => table.Title).HasColumnName("title").HasMaxLength(200);
        builder.Property(table => table.Content).HasColumnName("content");
        builder.Property(table => table.Settings).HasColumnName("settings");
        builder.Property(table => table.StyleId).HasColumnName("style_id").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.IsShare).HasColumnName("is_share").HasDefaultValue(0);
        builder.Property(table => table.UpdatedAt).HasColumnName("updated_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasMaxLength(10).HasDefaultValue(0);
        
    }
}