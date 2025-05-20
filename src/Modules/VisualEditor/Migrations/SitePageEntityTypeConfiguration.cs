using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.VisualEditor.Entities;

namespace NetDream.Modules.VisualEditor.Migrations;
public class SitePageEntityTypeConfiguration : IEntityTypeConfiguration<SitePageEntity>
{
    public void Configure(EntityTypeBuilder<SitePageEntity> builder)
    {
        builder.ToTable("tpl_site_page", table => table.HasComment("自定义站点页面"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.SiteId).HasColumnName("site_id").HasMaxLength(10);
        builder.Property(table => table.ComponentId).HasColumnName("component_id").HasMaxLength(10);
        builder.Property(table => table.Type).HasColumnName("type").HasDefaultValue(0);
        builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(100);
        builder.Property(table => table.Title).HasColumnName("title").HasMaxLength(200).HasDefaultValue("New Page");
        builder.Property(table => table.Keywords).HasColumnName("keywords").HasMaxLength(255);
        builder.Property(table => table.Thumb).HasColumnName("thumb").HasMaxLength(255);
        builder.Property(table => table.Description).HasColumnName("description").HasMaxLength(255);
        builder.Property(table => table.Settings).HasColumnName("settings");
        builder.Property(table => table.Position).HasColumnName("position").HasDefaultValue(10);
        builder.Property(table => table.Status).HasColumnName("status").HasDefaultValue(0).HasComment("发布状态");
        builder.Property(table => table.UpdatedAt).HasColumnName("updated_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasMaxLength(10).HasDefaultValue(0);
        
    }
}