using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Comment.Entities;

namespace NetDream.Modules.Comment.Migrations
{
    public class CommentEntityTypeConfiguration : IEntityTypeConfiguration<CommentEntity>
    {
        public void Configure(EntityTypeBuilder<CommentEntity> builder)
        {
            builder.ToTable("comment", table => table.HasComment("评论"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Content).HasColumnName("content");
            builder.Property(table => table.ExtraRule).HasColumnName("extra_rule").HasMaxLength(300).HasDefaultValue(string.Empty)
                .HasComment("内容的一些附加规则");
            builder.Property(table => table.ParentId).HasColumnName("parent_id");
            builder.Property(table => table.UserId).HasColumnName("user_id").HasDefaultValue(0);
            builder.Property(table => table.ItemId).HasColumnName("item_id");
            builder.Property(table => table.ItemType).HasColumnName("item_type").HasDefaultValue(0);
            builder.Property(table => table.Score).HasColumnName("score").HasDefaultValue(0);
            builder.Property(table => table.FromId).HasColumnName("from_id").HasDefaultValue(0);
            builder.Property(table => table.FromType).HasColumnName("from_type").HasDefaultValue(0);
            builder.Property(table => table.GuestName).HasColumnName("guest_name").HasDefaultValue(string.Empty);
            builder.Property(table => table.GuestEmail).HasColumnName("guest_email").HasDefaultValue(string.Empty);
            builder.Property(table => table.GuestUrl).HasColumnName("guest_url").HasDefaultValue(string.Empty);


            builder.Property(table => table.AgreeCount).HasColumnName("agree_count").HasDefaultValue(0);
            builder.Property(table => table.DisagreeCount).HasColumnName("disagree_count").HasDefaultValue(0);
            builder.Property(table => table.Status).HasColumnName("status").HasMaxLength(1).HasDefaultValue(0).HasComment("审核状态");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
