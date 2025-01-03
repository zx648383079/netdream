using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Book.Entities;

namespace NetDream.Modules.Book.Migrations
{
    public class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<CategoryEntity>
    {
        public void Configure(EntityTypeBuilder<CategoryEntity> builder)
        {
            builder.ToTable("book_category", table => table.HasComment("小说分类"));
            builder.HasKey(table => table.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.HasIndex(table => table.Name).IsUnique();
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(100).HasComment("分类名");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");

            builder.HasMany(p => p.Books)
                   .WithOne(b => b.Category)
                   .HasPrincipalKey(p => p.Id)
                   .HasForeignKey(b => b.CatId);
        }
    }
}
