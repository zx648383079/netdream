using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.ResourceStore.Entities;

namespace NetDream.Modules.ResourceStore.Migrations;
public class ResourceEntityTypeConfiguration : IEntityTypeConfiguration<ResourceEntity>
{
    public void Configure(EntityTypeBuilder<ResourceEntity> builder)
    {
        builder.ToTable("res_resource", table => table.HasComment(""));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.Title).HasColumnName("title").HasMaxLength(200);
        builder.Property(table => table.Description).HasColumnName("description").HasMaxLength(255);
        builder.Property(table => table.Keywords).HasColumnName("keywords").HasMaxLength(255);
        builder.Property(table => table.Thumb).HasColumnName("thumb").HasMaxLength(255);
        builder.Property(table => table.Content).HasColumnName("content");
        builder.Property(table => table.Size).HasColumnName("size").HasMaxLength(20).HasDefaultValue("0");
        builder.Property(table => table.Score).HasColumnName("score").HasMaxLength(5).HasDefaultValue("10");
        builder.Property(table => table.UserId).HasColumnName("user_id").HasMaxLength(10);
        builder.Property(table => table.PreviewType).HasColumnName("preview_type").HasDefaultValue(0).HasComment("预览文件类型");
        builder.Property(table => table.CatId).HasColumnName("cat_id").HasMaxLength(10);
        builder.Property(table => table.Price).HasColumnName("price").HasMaxLength(10).HasDefaultValue(0).HasComment("价格");
        builder.Property(table => table.IsCommercial).HasColumnName("is_commercial").HasDefaultValue(0).HasComment("是否允许商用");
        builder.Property(table => table.IsReprint).HasColumnName("is_reprint").HasDefaultValue(0).HasComment("是否允许转载");
        builder.Property(table => table.CommentCount).HasColumnName("comment_count").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.ViewCount).HasColumnName("view_count").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.DownloadCount).HasColumnName("download_count").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.UpdatedAt).HasColumnName("updated_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasMaxLength(10).HasDefaultValue(0);
        
    }
}