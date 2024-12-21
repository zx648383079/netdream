using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Blog.Entities;

namespace NetDream.Modules.Blog.Migrations
{
    public class CommentEntityTypeConfiguration : IEntityTypeConfiguration<CommentEntity>
    {
        public void Configure(EntityTypeBuilder<CommentEntity> builder)
        {
            builder.ToTable("blog_comment", table => table.HasComment(""));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Content).HasColumnName("content");
            builder.Property(table => table.ExtraRule).HasColumnName("extra_rule").HasMaxLength(300).HasDefaultValue(string.Empty)
                .HasComment("内容的一些附加规则");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(30).HasDefaultValue(string.Empty);
            builder.Property(table => table.Email).HasColumnName("email").HasMaxLength(50).HasDefaultValue(string.Empty);
            builder.Property(table => table.Url).HasColumnName("url").HasMaxLength(50).HasDefaultValue(string.Empty);
            builder.Property(table => table.ParentId).HasColumnName("parent_id").HasDefaultValue(0);
            builder.Property(table => table.Position).HasColumnName("position").HasDefaultValue(1);
            builder.Property(table => table.UserId).HasColumnName("user_id").HasDefaultValue(0);
            builder.Property(table => table.BlogId).HasColumnName("blog_id");
            builder.Property(table => table.Ip).HasColumnName("ip").HasMaxLength(120).HasDefaultValue(string.Empty);
            builder.Property(table => table.Agent).HasColumnName("agent").HasDefaultValue(string.Empty);
            builder.Property(table => table.AgreeCount).HasColumnName("agree_count").HasDefaultValue(0);
            builder.Property(table => table.DisagreeCount).HasColumnName("disagree_count").HasDefaultValue(0);
            builder.Property(table => table.Approved).HasColumnName("approved").HasMaxLength(1).HasDefaultValue(2);
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
