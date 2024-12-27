using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Book.Entities;

namespace NetDream.Modules.Book.Migrations
{
    public class BookEntityTypeConfiguration : IEntityTypeConfiguration<BookEntity>
    {
        public void Configure(EntityTypeBuilder<BookEntity> builder)
        {
            builder.ToTable("book", table => table.HasComment("小说"));
            builder.HasKey(table => table.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.HasIndex(table => table.Name).IsUnique();
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(100).HasComment("书名");
            builder.Property(table => table.Cover).HasColumnName("cover").HasMaxLength(200).HasDefaultValue(string.Empty).HasComment("封面");
            builder.Property(table => table.Description).HasColumnName("description").HasMaxLength(200).HasDefaultValue(string.Empty).HasComment("简介");
            builder.Property(table => table.AuthorId).HasColumnName("author_id").HasDefaultValue(0).HasComment("作者");
            builder.Property(table => table.UserId).HasColumnName("user_id").HasDefaultValue(0);
            builder.Property(table => table.Classify).HasColumnName("classify").HasMaxLength(2).HasDefaultValue(0).HasComment("小说分级");
            builder.Property(table => table.CatId).HasColumnName("cat_id").HasDefaultValue(0).HasComment("分类");
            builder.Property(table => table.Size).HasColumnName("size").HasDefaultValue(0).HasComment("总字数");
            builder.Property(table => table.ClickCount).HasColumnName("click_count").HasDefaultValue(0).HasComment("点击数");
            builder.Property(table => table.RecommendCount).HasColumnName("recommend_count")
                .HasDefaultValue(0).HasComment("推荐数");
            builder.Property(table => table.OverAt).HasColumnName("over_at").HasComment("完本日期");
            builder.Property(table => table.Status).HasColumnName("status").HasMaxLength(2).HasDefaultValue(0);
            builder.Property(table => table.SourceType).HasColumnName("source_type").HasMaxLength(2).HasDefaultValue(0).HasComment("来源类型，原创或转载");
            builder.Property(table => table.DeletedAt).HasColumnName("deleted_at");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");

            builder.HasMany(p => p.Chapters)
                  .WithOne(b => b.Book)
                  .HasPrincipalKey(p => p.Id)
                  .HasForeignKey(b => b.BookId);

            builder.HasMany(p => p.Sources)
                  .WithOne(b => b.Book)
                  .HasPrincipalKey(p => p.Id)
                  .HasForeignKey(b => b.BookId);
        }
    }
}
