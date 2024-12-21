using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Migrations
{
    public class ArticleEntityTypeConfiguration : IEntityTypeConfiguration<ArticleEntity>
    {
        public void Configure(EntityTypeBuilder<ArticleEntity> builder)
        {
            builder.ToTable("Article", table => table.HasComment(""));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.CatId).HasColumnName("cat_id");
            builder.Property(table => table.Title).HasColumnName("title").HasMaxLength(100).HasComment("ÎÄÕÂÃû");
            builder.Property(table => table.Keywords).HasColumnName("keywords").HasMaxLength(200).HasDefaultValue(string.Empty).HasComment("¹Ø¼ü×Ö");
            builder.Property(table => table.Thumb).HasColumnName("thumb").HasMaxLength(200).HasDefaultValue(string.Empty).HasComment("ËõÂÔÍ¼");
            builder.Property(table => table.Description).HasColumnName("description").HasMaxLength(200).HasDefaultValue(string.Empty).HasComment("¹Ø¼ü×Ö");
            builder.Property(table => table.Brief).HasColumnName("brief").HasMaxLength(200).HasDefaultValue(string.Empty).HasComment("¼ò½é");
            builder.Property(table => table.Url).HasColumnName("url").HasMaxLength(200).HasDefaultValue(string.Empty).HasComment("Á´½Ó");
            builder.Property(table => table.File).HasColumnName("file").HasMaxLength(200).HasDefaultValue(string.Empty).HasComment("ÏÂÔØÄÚÈÝ");
            builder.Property(table => table.Content).HasColumnName("content").HasComment("ÄÚÈÝ");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
