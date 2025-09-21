using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.MicroBlog.Entities;

namespace NetDream.Modules.MicroBlog.Migrations
{
    public class BlogEntityTypeConfiguration : IEntityTypeConfiguration<BlogEntity>
    {
        public void Configure(EntityTypeBuilder<BlogEntity> builder)
        {
            builder.ToTable("micro_blog", table => table.HasComment(""));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.ZoneId).HasColumnName("zone_id").HasDefaultValue(0);
            builder.Property(table => table.Content).HasColumnName("content").HasMaxLength(140);
            builder.Property(table => table.ExtraRule).HasColumnName("extra_rule").HasMaxLength(500).HasDefaultValue(string.Empty)
                .HasComment("内容的一些附加规则");
            builder.Property(table => table.OpenType).HasColumnName("open_type").HasMaxLength(1).HasDefaultValue(0);
            builder.Property(table => table.RecommendCount).HasColumnName("recommend_count").HasDefaultValue(0).HasComment("推荐数");
            builder.Property(table => table.CollectCount).HasColumnName("collect_count").HasDefaultValue(0).HasComment("收藏数");
            builder.Property(table => table.ForwardCount).HasColumnName("forward_count").HasDefaultValue(0).HasComment("转发数");
            builder.Property(table => table.CommentCount).HasColumnName("comment_count").HasDefaultValue(0).HasComment("评论数");
            builder.Property(table => table.ForwardId).HasColumnName("forward_id").HasDefaultValue(0).HasComment("转发的源id");
            builder.Property(table => table.Source).HasColumnName("source").HasMaxLength(30).HasDefaultValue(string.Empty).HasComment("来源");
            builder.Property(table => table.Status).HasColumnName("status").HasDefaultValue(0).HasComment("审核状态");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
