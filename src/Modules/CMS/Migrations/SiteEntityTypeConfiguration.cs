using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.CMS.Entities;
using NetDream.Modules.CMS.Repositories;

namespace NetDream.Modules.CMS.Migrations
{
    public class SiteEntityTypeConfiguration : IEntityTypeConfiguration<SiteEntity>
    {
        public void Configure(EntityTypeBuilder<SiteEntity> builder)
        {
            builder.ToTable("Site", table => table.HasComment(""));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Title).HasColumnName("title");
            builder.Property(table => table.Keywords).HasColumnName("keywords").HasDefaultValue(string.Empty);
            builder.Property(table => table.Description).HasColumnName("description").HasDefaultValue(string.Empty);
            builder.Property(table => table.Logo).HasColumnName("logo").HasDefaultValue(string.Empty);
            builder.Property(table => table.Language).HasColumnName("language").HasMaxLength(10).HasDefaultValue(string.Empty);
            builder.Property(table => table.Theme).HasColumnName("theme").HasMaxLength(100);
            builder.Property(table => table.MatchRule).HasColumnName("match_rule").HasMaxLength(100).HasDefaultValue(string.Empty);
            builder.Property(table => table.IsDefault).HasColumnName("is_default").HasDefaultValue(0);
            builder.Property(table => table.Status).HasColumnName("status").HasMaxLength(1).HasDefaultValue(SiteRepository.PUBLISH_STATUS_POSTED)
                .HasComment("·¢²¼×´Ì¬");
            builder.Property(table => table.Options).HasColumnName("options");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
