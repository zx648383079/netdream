using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Book.Entities;

namespace NetDream.Modules.Book.Migrations
{
    public class ListEntityTypeConfiguration : IEntityTypeConfiguration<ListEntity>
    {
        public void Configure(EntityTypeBuilder<ListEntity> builder)
        {
            builder.ToTable("book_list", table => table.HasComment("Êéµ¥"));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.Title).HasColumnName("title").HasMaxLength(50);
            builder.Property(table => table.Description).HasColumnName("description").HasMaxLength(200).HasDefaultValue(string.Empty);
            builder.Property(table => table.BookCount).HasColumnName("book_count").HasDefaultValue(0);
            builder.Property(table => table.ClickCount).HasColumnName("click_count").HasDefaultValue(0);
            builder.Property(table => table.CollectCount).HasColumnName("collect_count").HasDefaultValue(0);
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");

            builder.HasMany(p => p.Items)
                .WithOne(b => b.List)
                .HasPrincipalKey(p => p.Id)
                .HasForeignKey(b => b.ListId);
        }
    }
}
