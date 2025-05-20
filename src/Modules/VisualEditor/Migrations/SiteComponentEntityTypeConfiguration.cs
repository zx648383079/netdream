using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.VisualEditor.Entities;

namespace NetDream.Modules.VisualEditor.Migrations;
public class SiteComponentEntityTypeConfiguration : IEntityTypeConfiguration<SiteComponentEntity>
{
    public void Configure(EntityTypeBuilder<SiteComponentEntity> builder)
    {
        builder.ToTable("tpl_site_component", table => table.HasComment("站点需要的页面和部件"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.ComponentId).HasColumnName("component_id").HasMaxLength(10);
        builder.Property(table => table.SiteId).HasColumnName("site_id").HasMaxLength(10);
        builder.Property(table => table.CatId).HasColumnName("cat_id").HasMaxLength(10);
        builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(30);
        builder.Property(table => table.Description).HasColumnName("description").HasMaxLength(200);
        builder.Property(table => table.Thumb).HasColumnName("thumb").HasMaxLength(100);
        builder.Property(table => table.Type).HasColumnName("type").HasDefaultValue(0).HasComment("部件类型");
        builder.Property(table => table.Author).HasColumnName("author").HasMaxLength(20);
        builder.Property(table => table.Version).HasColumnName("version").HasMaxLength(10);
        builder.Property(table => table.Path).HasColumnName("path").HasMaxLength(200);
        builder.Property(table => table.Editable).HasColumnName("editable").HasDefaultValue(0).HasComment("是否有编辑表单");
        builder.Property(table => table.AliasName).HasColumnName("alias_name").HasMaxLength(20);
        builder.Property(table => table.UpdatedAt).HasColumnName("updated_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.Dependencies).HasColumnName("dependencies").HasMaxLength(500).HasDefaultValue("依赖的脚本和css文件");
        
    }
}