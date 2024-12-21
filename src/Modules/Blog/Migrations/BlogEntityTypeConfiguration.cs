using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Blog.Entities;
using NetDream.Modules.Blog.Repositories;

namespace NetDream.Modules.Blog.Migrations
{
    public class BlogEntityTypeConfiguration : IEntityTypeConfiguration<BlogEntity>
    {
        public void Configure(EntityTypeBuilder<BlogEntity> builder)
        {
            builder.ToTable("blog", table => table.HasComment(""));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Title).HasColumnName("title").HasMaxLength(200);
            builder.Property(table => table.Description).HasColumnName("description").HasDefaultValue(string.Empty);
            builder.Property(table => table.Keywords).HasColumnName("keywords").HasDefaultValue(string.Empty);
            builder.Property(table => table.ParentId).HasColumnName("parent_id").HasDefaultValue(0);
            builder.Property(table => table.ProgrammingLanguage).HasColumnName("programming_language").HasMaxLength(20)
                .HasDefaultValue(string.Empty).HasComment("编程语言");
            builder.Property(table => table.Language).HasColumnName("language").HasComment("多语言配置");
            builder.Property(table => table.Thumb).HasColumnName("thumb").HasDefaultValue(string.Empty);
            builder.Property(table => table.EditType).HasColumnName("edit_type").HasMaxLength(1).HasDefaultValue(PublishRepository.EDIT_HTML).HasComment("编辑器类型");
            builder.Property(table => table.Content).HasColumnName("content");
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.TermId).HasColumnName("term_id");
            builder.Property(table => table.Type).HasColumnName("type").HasDefaultValue(PublishRepository.TYPE_ORIGINAL).HasComment("原创或转载");
            builder.Property(table => table.RecommendCount).HasColumnName("recommend_count").HasDefaultValue(0);
            builder.Property(table => table.CommentCount).HasColumnName("comment_count").HasDefaultValue(0);
            builder.Property(table => table.ClickCount).HasColumnName("click_count").HasDefaultValue(0);
            builder.Property(table => table.OpenType).HasColumnName("open_type").HasMaxLength(1).HasDefaultValue(PublishRepository.OPEN_PUBLIC).HasComment("公开类型");
            builder.Property(table => table.OpenRule).HasColumnName("open_rule").HasMaxLength(20).HasDefaultValue(string.Empty).HasComment("类型匹配的值");
            builder.Property(table => table.PublishStatus).HasColumnName("publish_status").HasMaxLength(1).HasDefaultValue(PublishRepository.PUBLISH_STATUS_DRAFT).HasComment("发布状态");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
