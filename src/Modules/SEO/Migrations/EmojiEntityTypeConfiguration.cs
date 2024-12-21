using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.SEO.Entities;

namespace NetDream.Modules.SEO.Migrations
{
    public class EmojiEntityTypeConfiguration : IEntityTypeConfiguration<EmojiEntity>
    {
        public void Configure(EntityTypeBuilder<EmojiEntity> builder)
        {
            builder.ToTable("seo_emoji", table => table.HasComment("表情"));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.CatId).HasColumnName("cat_id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(30);
            builder.Property(table => table.Type).HasColumnName("type").HasMaxLength(1).HasDefaultValue(0).HasComment("图片或文字");
            builder.Property(table => table.Content).HasColumnName("content").HasDefaultValue(string.Empty);
        }
    }
}
