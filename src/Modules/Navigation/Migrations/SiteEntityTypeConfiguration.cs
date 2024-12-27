using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Navigation.Entities;

namespace NetDream.Modules.Navigation.Migrations
{
    public class SiteEntityTypeConfiguration : IEntityTypeConfiguration<SiteEntity>
    {
        public void Configure(EntityTypeBuilder<SiteEntity> builder)
        {
            builder.ToTable("search_site", table => table.HasComment("站点表"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Schema).HasColumnName("schema").HasMaxLength(10).HasDefaultValue("https");
            builder.Property(table => table.Domain).HasColumnName("domain").HasMaxLength(100);
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(30);
            builder.Property(table => table.Logo).HasColumnName("logo").HasDefaultValue(string.Empty);
            builder.Property(table => table.Description).HasColumnName("description").HasDefaultValue(string.Empty);
            builder.Property(table => table.CatId).HasColumnName("cat_id").HasDefaultValue(0);
            builder.Property(table => table.UserId).HasColumnName("user_id").HasDefaultValue(0);
            builder.Property(table => table.TopType).HasColumnName("top_type").HasMaxLength(1).HasDefaultValue(0).HasComment("推荐类型");
            builder.Property(table => table.Score).HasColumnName("score").HasMaxLength(1).HasDefaultValue(60).HasComment("站点评分/百分制");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
