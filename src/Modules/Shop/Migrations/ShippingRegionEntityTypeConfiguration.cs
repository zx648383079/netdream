using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Migrations
{
    public class ShippingRegionEntityTypeConfiguration : IEntityTypeConfiguration<ShippingRegionEntity>
    {
        public void Configure(EntityTypeBuilder<ShippingRegionEntity> builder)
        {
            builder.ToTable("ShippingRegion", table => table.HasComment(""));
            builder.Property(table => table.ShippingId).HasColumnName("shipping_id");
            builder.Property(table => table.GroupId).HasColumnName("group_id");
            builder.Property(table => table.RegionId).HasColumnName("region_id");
        }
    }
}
