using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Migrations
{
    public class OrderCouponEntityTypeConfiguration : IEntityTypeConfiguration<OrderCouponEntity>
    {
        public void Configure(EntityTypeBuilder<OrderCouponEntity> builder)
        {
            builder.ToTable("OrderCoupon", table => table.HasComment(""));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.OrderId).HasColumnName("order_id");
            builder.Property(table => table.CouponId).HasColumnName("coupon_id");
            builder.Property(table => table.Name).HasColumnName("name").HasDefaultValue(string.Empty);
            builder.Property(table => table.Type).HasColumnName("type").HasDefaultValue(string.Empty);
        }
    }
}
