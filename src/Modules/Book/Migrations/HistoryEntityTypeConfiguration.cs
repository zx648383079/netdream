using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Book.Entities;

namespace NetDream.Modules.Book.Migrations
{
    public class HistoryEntityTypeConfiguration : IEntityTypeConfiguration<HistoryEntity>
    {
        public void Configure(EntityTypeBuilder<HistoryEntity> builder)
        {
            builder.ToTable("book_history", table => table.HasComment("小说阅读历史"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.BookId).HasColumnName("book_id");
            builder.Property(table => table.ChapterId).HasColumnName("chapter_id").HasDefaultValue(0);
            builder.Property(table => table.Progress).HasColumnName("progress").HasMaxLength(1).HasDefaultValue(0);
            builder.Property(table => table.SourceId).HasColumnName("source_id").HasDefaultValue(0);
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
