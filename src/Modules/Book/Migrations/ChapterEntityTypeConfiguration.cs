using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Book.Entities;
using NetDream.Modules.Book.Repositories;

namespace NetDream.Modules.Book.Migrations
{
    public class ChapterEntityTypeConfiguration : IEntityTypeConfiguration<ChapterEntity>
    {
        public void Configure(EntityTypeBuilder<ChapterEntity> builder)
        {
            builder.ToTable("book_chapter", table => table.HasComment("小说章节"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.BookId).HasColumnName("book_id");
            builder.Property(table => table.Type).HasColumnName("type").HasMaxLength(1).HasDefaultValue(ChapterRepository.TYPE_FREE_CHAPTER).HasComment("章节类型，是分卷还是章节");
            builder.Property(table => table.Title).HasColumnName("title").HasMaxLength(200).HasComment("标题");
            builder.Property(table => table.ParentId).HasColumnName("parent_id").HasDefaultValue(0);
            builder.Property(table => table.Price).HasColumnName("price").HasDefaultValue(0);
            builder.Property(table => table.Status).HasColumnName("status").HasMaxLength(2).HasDefaultValue(0);
            builder.Property(table => table.Position).HasColumnName("position").HasMaxLength(4).HasDefaultValue(99);
            builder.Property(table => table.Size).HasColumnName("size").HasDefaultValue(0).HasComment("字数");
            builder.Property(table => table.DeletedAt).HasColumnName("deleted_at");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");

            builder.HasOne(i => i.Body)
                .WithOne(p => p.Chapter)
                .HasForeignKey<ChapterEntity>();
        }
    }
}
