using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Blog.Entities;

namespace NetDream.Modules.Blog.Migrations
{
    public class MetaEntityTypeConfiguration : IEntityTypeConfiguration<MetaEntity>
    {
        public void Configure(EntityTypeBuilder<MetaEntity> builder)
        {
            builder.ToTable("blog_meta", table => table.HasComment(""));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.BlogId).HasColumnName("blog_id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(100);
            builder.Property(table => table.Content).HasColumnName("content");
        }
    }
}
