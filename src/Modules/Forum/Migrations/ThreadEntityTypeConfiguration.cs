using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Forum.Entities;

namespace NetDream.Modules.Forum.Migrations
{
    public class ThreadEntityTypeConfiguration : IEntityTypeConfiguration<ThreadEntity>
    {
        public void Configure(EntityTypeBuilder<ThreadEntity> builder)
        {
            builder.ToTable("bbs_thread", table => table.HasComment(""));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.ForumId).HasColumnName("forum_id");
            builder.Property(table => table.ZoneId).HasColumnName("zone_id").HasDefaultValue(0);
            builder.Property(table => table.ClassifyId).HasColumnName("classify_id").HasDefaultValue(0);
            builder.Property(table => table.Title).HasColumnName("title").HasMaxLength(200).HasComment("主题");
            builder.Property(table => table.UserId).HasColumnName("user_id").HasComment("发送用户");
            builder.Property(table => table.ViewCount).HasColumnName("view_count").HasDefaultValue(0).HasComment("查看数");
            builder.Property(table => table.PostCount).HasColumnName("post_count").HasDefaultValue(0).HasComment("回帖数");
            builder.Property(table => table.CollectCount).HasColumnName("collect_count").HasDefaultValue(0).HasComment("关注数");
            builder.Property(table => table.IsHighlight).HasColumnName("is_highlight").HasDefaultValue(0)
                .HasComment("是否高亮");
            builder.Property(table => table.IsDigest).HasColumnName("is_digest").HasDefaultValue(0)
                .HasComment("是否精华");
            builder.Property(table => table.IsClosed).HasColumnName("is_closed").HasDefaultValue(0)
                .HasComment("是否关闭");
            builder.Property(table => table.TopType).HasColumnName("top_type").HasMaxLength(1).HasDefaultValue(0)
                .HasComment("置顶类型，1 本版置顶 2 分类置顶 3 全局置顶");
            builder.Property(table => table.IsPrivatePost).HasColumnName("is_private_post").HasDefaultValue(0).HasComment("是否仅楼主可见");
            builder.Property(table => table.Status).HasColumnName("status").HasDefaultValue(0).HasComment("审核状态");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
