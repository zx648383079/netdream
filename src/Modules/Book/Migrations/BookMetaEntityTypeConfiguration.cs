using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Book.Entities;

namespace NetDream.Modules.Book.Migrations
{
    public class BookMetaEntityTypeConfiguration : IEntityTypeConfiguration<BookMetaEntity>
    {
        public void Configure(EntityTypeBuilder<BookMetaEntity> builder)
        {
            builder.ToTable("book_meta", table => table.HasComment("小说附加信息"));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.TargetId).HasColumnName("target_id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(50);
            builder.Property(table => table.Content).HasColumnName("content").HasDefaultValue(string.Empty);
        }
    }
}
