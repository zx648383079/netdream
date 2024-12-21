using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Migrations
{
    public class WarehouseRegionEntityTypeConfiguration : IEntityTypeConfiguration<WarehouseRegionEntity>
    {
        public void Configure(EntityTypeBuilder<WarehouseRegionEntity> builder)
        {
            builder.ToTable("WarehouseRegion", table => table.HasComment(""));
            builder.HasKey("id");
            builder.Property(table => table.WarehouseId).HasColumnName("warehouse_id");
            builder.Property(table => table.RegionId).HasColumnName("region_id");
        }
    }
}
