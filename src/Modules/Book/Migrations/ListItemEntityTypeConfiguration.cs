using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Book.Entities;

namespace NetDream.Modules.Book.Migrations
{
    public class ListItemEntityTypeConfiguration : IEntityTypeConfiguration<ListItemEntity>
    {
        public void Configure(EntityTypeBuilder<ListItemEntity> builder)
        {
            builder.ToTable("book_list_item", table => table.HasComment("Êéµ¥Êé¼®"));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.ListId).HasColumnName("list_id");
            builder.Property(table => table.BookId).HasColumnName("book_id");
            builder.Property(table => table.Remark).HasColumnName("remark").HasMaxLength(200).HasDefaultValue(string.Empty);
            builder.Property(table => table.Star).HasColumnName("star").HasMaxLength(1).HasDefaultValue(10);
            builder.Property(table => table.AgreeCount).HasColumnName("agree_count").HasDefaultValue(0);
            builder.Property(table => table.DisagreeCount).HasColumnName("disagree_count").HasDefaultValue(0);
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
