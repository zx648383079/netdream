using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Document.Entities;

namespace NetDream.Modules.Document.Migrations
{
    public class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<CategoryEntity>
    {
        public void Configure(EntityTypeBuilder<CategoryEntity> builder)
        {
            builder.ToTable("doc_category", table => table.HasComment("·ÖÀà"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(30);
            builder.Property(table => table.Icon).HasColumnName("icon").HasDefaultValue(string.Empty);
            builder.Property(table => table.ParentId).HasColumnName("parent_id").HasDefaultValue(0);
        }
    }
}
