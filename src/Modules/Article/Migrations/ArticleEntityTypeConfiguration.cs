using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Article.Entities;
using NetDream.Modules.Article.Repositories;
using NetDream.Shared.Models;

namespace NetDream.Modules.Article.Migrations
{
    public class ArticleEntityTypeConfiguration : IEntityTypeConfiguration<ArticleEntity>
    {
        public void Configure(EntityTypeBuilder<ArticleEntity> builder)
        {
            builder.ToTable("article", table => table.HasComment("文章"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Title).HasColumnName("title").HasMaxLength(200);
            builder.Property(table => table.Description).HasColumnName("description").HasDefaultValue(string.Empty);
            builder.Property(table => table.Keywords).HasColumnName("keywords").HasDefaultValue(string.Empty);
            builder.Property(table => table.ParentId).HasColumnName("parent_id").HasDefaultValue(0);
            builder.Property(table => table.Language).HasColumnName("language").HasComment("多语言配置");
            builder.Property(table => table.Thumb).HasColumnName("thumb").HasDefaultValue(string.Empty);
            builder.Property(table => table.EditType).HasColumnName("edit_type").HasMaxLength(1).HasDefaultValue(TextType.Html).HasComment("编辑器类型");
            builder.Property(table => table.Content).HasColumnName("content");
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.CatId).HasColumnName("cat_id");
            builder.Property(table => table.Type).HasColumnName("type").HasDefaultValue(ModuleTargetType.Article).HasComment("所属模块");
            builder.Property(table => table.OriginalType).HasColumnName("original_type").HasDefaultValue(ArticleRepository.TYPE_ORIGINAL).HasComment("原创或转载");
            builder.Property(table => table.LikeCount).HasColumnName("like_count").HasDefaultValue(0);
            builder.Property(table => table.CommentCount).HasColumnName("comment_count").HasDefaultValue(0);
            builder.Property(table => table.ClickCount).HasColumnName("click_count").HasDefaultValue(0);
            builder.Property(table => table.OpenType).HasColumnName("open_type").HasMaxLength(1).HasDefaultValue(ArticleRepository.OPEN_PUBLIC).HasComment("公开类型");
            builder.Property(table => table.OpenRule).HasColumnName("open_rule").HasMaxLength(20).HasDefaultValue(string.Empty).HasComment("类型匹配的值");
            builder.Property(table => table.PublishStatus).HasColumnName("publish_status").HasMaxLength(1).HasDefaultValue(ArticleRepository.PUBLISH_STATUS_DRAFT).HasComment("发布状态");
            builder.Property(table => table.Status).HasColumnName("status").HasMaxLength(1)
                .HasDefaultValue(ReviewStatus.None).HasComment("发布状态");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
