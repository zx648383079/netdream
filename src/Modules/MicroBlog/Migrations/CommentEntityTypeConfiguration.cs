using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.MicroBlog.Entities;

namespace NetDream.Modules.MicroBlog.Migrations
{
    public class CommentEntityTypeConfiguration : IEntityTypeConfiguration<CommentEntity>
    {
        public void Configure(EntityTypeBuilder<CommentEntity> builder)
        {
            builder.ToTable("micro_comment", table => table.HasComment(""));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Content).HasColumnName("content");
            builder.Property(table => table.ExtraRule).HasColumnName("extra_rule").HasMaxLength(300).HasDefaultValue(string.Empty)
                .HasComment("内容的一些附加规则");
            builder.Property(table => table.ParentId).HasColumnName("parent_id");
            builder.Property(table => table.UserId).HasColumnName("user_id").HasDefaultValue(0);
            builder.Property(table => table.TargetId).HasColumnName("target_id");
            builder.Property(table => table.AgreeCount).HasColumnName("agree_count").HasDefaultValue(0);
            builder.Property(table => table.DisagreeCount).HasColumnName("disagree_count").HasDefaultValue(0);
            builder.Property(table => table.Status).HasColumnName("status").HasMaxLength(1).HasDefaultValue(0).HasComment("审核状态");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
