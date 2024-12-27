using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Migrations
{
    public class OrderAddressEntityTypeConfiguration : IEntityTypeConfiguration<OrderAddressEntity>
    {
        public void Configure(EntityTypeBuilder<OrderAddressEntity> builder)
        {
            builder.ToTable("OrderAddress", table => table.HasComment(""));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.OrderId).HasColumnName("order_id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(30);
            builder.Property(table => table.RegionId).HasColumnName("region_id");
            builder.Property(table => table.RegionName).HasColumnName("region_name");
            builder.Property(table => table.Tel).HasColumnName("tel").HasMaxLength(11);
            builder.Property(table => table.Address).HasColumnName("address");
            builder.Property(table => table.BestTime).HasColumnName("best_time").HasDefaultValue(string.Empty);
        }
    }
}
