using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.SEO.Entities;

namespace NetDream.Modules.SEO.Migrations
{
    public class EmojiCategoryEntityTypeConfiguration : IEntityTypeConfiguration<EmojiCategoryEntity>
    {
        public void Configure(EntityTypeBuilder<EmojiCategoryEntity> builder)
        {
            builder.ToTable("seo_emoji_category", table => table.HasComment("�������"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(20);
            builder.Property(table => table.Icon).HasColumnName("icon").HasDefaultValue(string.Empty);
            builder.HasMany(p => p.Items)
                .WithOne(b => b.Category)
                .HasPrincipalKey(p => p.Id)
                .HasForeignKey(b => b.CatId);
        }
    }
}
