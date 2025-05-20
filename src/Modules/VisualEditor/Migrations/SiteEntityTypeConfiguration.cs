using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.VisualEditor.Entities;

namespace NetDream.Modules.VisualEditor.Migrations;
public class SiteEntityTypeConfiguration : IEntityTypeConfiguration<SiteEntity>
{
    public void Configure(EntityTypeBuilder<SiteEntity> builder)
    {
        builder.ToTable("tpl_site", table => table.HasComment("自定义站点"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(100);
        builder.Property(table => table.UserId).HasColumnName("user_id").HasMaxLength(10);
        builder.Property(table => table.Title).HasColumnName("title").HasMaxLength(200).HasDefaultValue("New Site");
        builder.Property(table => table.Keywords).HasColumnName("keywords").HasMaxLength(255);
        builder.Property(table => table.Thumb).HasColumnName("thumb").HasMaxLength(255);
        builder.Property(table => table.Logo).HasColumnName("logo").HasMaxLength(255);
        builder.Property(table => table.Description).HasColumnName("description").HasMaxLength(255);
        builder.Property(table => table.Domain).HasColumnName("domain").HasMaxLength(50);
        builder.Property(table => table.DefaultPageId).HasColumnName("default_page_id").HasMaxLength(10).HasDefaultValue(0).HasComment("默认首页");
        builder.Property(table => table.IsShare).HasColumnName("is_share").HasDefaultValue(0).HasComment("是否共享，允许其他人复制");
        builder.Property(table => table.SharePrice).HasColumnName("share_price").HasMaxLength(10).HasDefaultValue(0).HasComment("共享是否需要购买");
        builder.Property(table => table.Status).HasColumnName("status").HasDefaultValue(0).HasComment("发布状态");
        builder.Property(table => table.UpdatedAt).HasColumnName("updated_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasMaxLength(10).HasDefaultValue(0);
        
    }
}