using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Migrations
{
    public class CommentEntityTypeConfiguration : IEntityTypeConfiguration<CommentEntity>
    {
        public void Configure(EntityTypeBuilder<CommentEntity> builder)
        {
            builder.ToTable("Comment", table => table.HasComment(""));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.ItemType).HasColumnName("item_type").HasMaxLength(2).HasDefaultValue(0);
            builder.Property(table => table.ItemId).HasColumnName("item_id");
            builder.Property(table => table.Title).HasColumnName("title");
            builder.Property(table => table.Content).HasColumnName("content");
            builder.Property(table => table.Rank).HasColumnName("rank").HasMaxLength(2).HasDefaultValue(10);
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
