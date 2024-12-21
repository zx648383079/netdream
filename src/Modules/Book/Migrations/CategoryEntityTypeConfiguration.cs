using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Book.Entities;

namespace NetDream.Modules.Book.Migrations
{
    public class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<CategoryEntity>
    {
        public void Configure(EntityTypeBuilder<CategoryEntity> builder)
        {
            builder.ToTable("book_category", table => table.HasComment("С˵����"));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.HasIndex("name").IsUnique();
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(100).HasComment("������");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
