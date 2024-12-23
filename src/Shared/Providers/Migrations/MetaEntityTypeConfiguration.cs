using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using NetDream.Shared.Providers.Entities;

namespace NetDream.Shared.Providers.Migrations
{
    public class MetaEntityTypeConfiguration(string prefix) : IEntityTypeConfiguration<MetaEntity>
    {
        public void Configure(EntityTypeBuilder<MetaEntity> builder)
        {
            builder.ToTable(prefix + "_meta", table => table.HasComment("附加内容表"));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.ItemId).HasColumnName("item_id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(100);
            builder.Property(table => table.Content).HasColumnName("content");
        }
    }
}
