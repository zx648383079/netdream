using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.SEO.Entities;

namespace NetDream.Modules.SEO.Migrations
{
    public class AgreementEntityTypeConfiguration : IEntityTypeConfiguration<AgreementEntity>
    {
        public void Configure(EntityTypeBuilder<AgreementEntity> builder)
        {
            builder.ToTable("seo_agreement", table => table.HasComment("服务协议"));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(50);
            builder.Property(table => table.Title).HasColumnName("title").HasMaxLength(100);
            builder.Property(table => table.Language).HasColumnName("language").HasComment("多语言配置");

            builder.Property(table => table.Description).HasColumnName("description").HasMaxLength(500).HasDefaultValue(string.Empty);
            builder.Property(table => table.Content).HasColumnName("content");
            builder.Property(table => table.Status).HasColumnName("status").HasMaxLength(1).HasDefaultValue(0);
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
