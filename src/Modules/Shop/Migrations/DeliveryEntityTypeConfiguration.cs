using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Migrations
{
    public class DeliveryEntityTypeConfiguration : IEntityTypeConfiguration<DeliveryEntity>
    {
        public void Configure(EntityTypeBuilder<DeliveryEntity> builder)
        {
            builder.ToTable("Delivery", table => table.HasComment(""));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.OrderId).HasColumnName("order_id");
            builder.Property(table => table.Status).HasColumnName("status").HasDefaultValue(0);
            builder.Property(table => table.ShippingId).HasColumnName("shipping_id").HasDefaultValue(0);
            builder.Property(table => table.ShippingName).HasColumnName("shipping_name").HasMaxLength(30).HasDefaultValue(0);
            builder.Property(table => table.ShippingFee).HasColumnName("shipping_fee").HasMaxLength(8).HasDefaultValue(0);
            builder.Property(table => table.LogisticsNumber).HasColumnName("logistics_number").HasMaxLength(30).HasDefaultValue(string.Empty).HasComment("物流单号");
            builder.Property(table => table.LogisticsContent).HasColumnName("logistics_content").HasComment("物流信息");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(30);
            builder.Property(table => table.RegionId).HasColumnName("region_id");
            builder.Property(table => table.RegionName).HasColumnName("region_name");
            builder.Property(table => table.Tel).HasColumnName("tel").HasMaxLength(11);
            builder.Property(table => table.Address).HasColumnName("address");
            builder.Property(table => table.BestTime).HasColumnName("best_time").HasDefaultValue(string.Empty);
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
