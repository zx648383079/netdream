using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Blog.Entities;

namespace NetDream.Modules.Blog.Migrations
{
    public class TagRelationshipEntityTypeConfiguration : IEntityTypeConfiguration<TagRelationshipEntity>
    {
        public void Configure(EntityTypeBuilder<TagRelationshipEntity> builder)
        {
            builder.ToTable("blog_tag_relationship", table => table.HasComment(""));
            builder.Property(table => table.TagId).HasColumnName("tag_id");
            builder.Property(table => table.BlogId).HasColumnName("blog_id");
            builder.Property(table => table.Position).HasColumnName("position").HasMaxLength(2).HasDefaultValue(99);
        }
    }
}
