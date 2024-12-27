using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Book.Entities;

namespace NetDream.Modules.Book.Migrations
{
    public class BuyLogEntityTypeConfiguration : IEntityTypeConfiguration<BuyLogEntity>
    {
        public void Configure(EntityTypeBuilder<BuyLogEntity> builder)
        {
            builder.ToTable("book_buy_log", table => table.HasComment("Ð¡Ëµ¹ºÂò¼ÇÂ¼"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.BookId).HasColumnName("book_id");
            builder.Property(table => table.ChapterId).HasColumnName("chapter_id");
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
