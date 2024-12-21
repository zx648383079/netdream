using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Book.Entities;

namespace NetDream.Modules.Book.Migrations
{
    public class ChapterBodyEntityTypeConfiguration : IEntityTypeConfiguration<ChapterBodyEntity>
    {
        public void Configure(EntityTypeBuilder<ChapterBodyEntity> builder)
        {
            builder.ToTable("book_chapter_body", table => table.HasComment("С˵�½�����"));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Content).HasColumnName("content").HasComment("����");
        }
    }
}
