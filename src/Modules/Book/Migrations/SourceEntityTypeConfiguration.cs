using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Book.Entities;

namespace NetDream.Modules.Book.Migrations
{
    public class SourceEntityTypeConfiguration : IEntityTypeConfiguration<SourceEntity>
    {
        public void Configure(EntityTypeBuilder<SourceEntity> builder)
        {
            builder.ToTable("book_source", table => table.HasComment("小说来源"));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.BookId).HasColumnName("book_id");
            builder.Property(table => table.SiteId).HasColumnName("site_id");
            builder.Property(table => table.Url).HasColumnName("url").HasMaxLength(200).HasComment("来源网址");
            builder.Property(table => table.DeletedAt).HasColumnName("deleted_at");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
