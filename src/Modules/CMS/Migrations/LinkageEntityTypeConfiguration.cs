using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.CMS.Entities;

namespace NetDream.Modules.CMS.Migrations
{
    public class LinkageEntityTypeConfiguration : IEntityTypeConfiguration<LinkageEntity>
    {
        public void Configure(EntityTypeBuilder<LinkageEntity> builder)
        {
            builder.ToTable("Linkage", table => table.HasComment(""));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(100);
            builder.Property(table => table.Type).HasColumnName("type").HasMaxLength(2).HasDefaultValue(0);
            builder.Property(table => table.Code).HasColumnName("code").HasMaxLength(20);
            builder.Property(table => table.Language).HasColumnName("language").HasMaxLength(10).HasDefaultValue(string.Empty);
        }
    }
}
