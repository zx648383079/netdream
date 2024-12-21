using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Migrations
{
    public class WarehouseEntityTypeConfiguration : IEntityTypeConfiguration<WarehouseEntity>
    {
        public void Configure(EntityTypeBuilder<WarehouseEntity> builder)
        {
            builder.ToTable("Warehouse", table => table.HasComment(""));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(30);
            builder.Property(table => table.Tel).HasColumnName("tel").HasMaxLength(30);
            builder.Property(table => table.LinkUser).HasColumnName("link_user").HasMaxLength(30).HasDefaultValue(string.Empty);
            builder.Property(table => table.Address).HasColumnName("address").HasDefaultValue(string.Empty);
            builder.Property(table => table.Longitude).HasColumnName("longitude").HasMaxLength(50).HasComment("¾­¶È");
            builder.Property(table => table.Latitude).HasColumnName("latitude").HasMaxLength(50).HasComment("Î³¶È");
            builder.Property(table => table.Remark).HasColumnName("remark").HasDefaultValue(string.Empty);
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
