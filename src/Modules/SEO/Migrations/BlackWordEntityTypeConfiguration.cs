using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.SEO.Entities;

namespace NetDream.Modules.SEO.Migrations
{
    public class BlackWordEntityTypeConfiguration : IEntityTypeConfiguration<BlackWordEntity>
    {
        public void Configure(EntityTypeBuilder<BlackWordEntity> builder)
        {
            builder.ToTable("seo_black_word", table => table.HasComment("Î¥½û´Ê"));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Words).HasColumnName("words");
            builder.Property(table => table.ReplaceWords).HasColumnName("replace_words").HasDefaultValue(string.Empty);
        }
    }
}
