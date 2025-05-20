using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.VisualEditor.Entities;

namespace NetDream.Modules.VisualEditor.Migrations;
public class SitePageWeightEntityTypeConfiguration : IEntityTypeConfiguration<SitePageWeightEntity>
{
    public void Configure(EntityTypeBuilder<SitePageWeightEntity> builder)
    {
        builder.ToTable("tpl_site_page_weight", table => table.HasComment("自定义页面组件及设置"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.PageId).HasColumnName("page_id").HasMaxLength(10);
        builder.Property(table => table.SiteId).HasColumnName("site_id").HasMaxLength(10);
        builder.Property(table => table.WeightId).HasColumnName("weight_id").HasMaxLength(10);
        builder.Property(table => table.ParentId).HasColumnName("parent_id").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.ParentIndex).HasColumnName("parent_index").HasDefaultValue(0).HasComment("在父元素那个位置上");
        builder.Property(table => table.Position).HasColumnName("position").HasMaxLength(5).HasDefaultValue(99);
        
    }
}