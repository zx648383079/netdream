using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.SEO.Entities;

namespace NetDream.Modules.SEO.Migrations
{
    public class EmojiCategoryEntityTypeConfiguration : IEntityTypeConfiguration<EmojiCategoryEntity>
    {
        public void Configure(EntityTypeBuilder<EmojiCategoryEntity> builder)
        {
            builder.ToTable("seo_emoji_category", table => table.HasComment("表情分类"));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(20);
            builder.Property(table => table.Icon).HasColumnName("icon").HasDefaultValue(string.Empty);
        }
    }
}
