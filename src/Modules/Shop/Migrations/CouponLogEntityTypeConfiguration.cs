using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Migrations
{
    public class CouponLogEntityTypeConfiguration : IEntityTypeConfiguration<CouponLogEntity>
    {
        public void Configure(EntityTypeBuilder<CouponLogEntity> builder)
        {
            builder.ToTable("CouponLog", table => table.HasComment("ÓÅ»ÝÈ¯¼ÇÂ¼"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.CouponId).HasColumnName("coupon_id");
            builder.Property(table => table.SerialNumber).HasColumnName("serial_number").HasMaxLength(30).HasDefaultValue(string.Empty);
            builder.Property(table => table.UserId).HasColumnName("user_id").HasDefaultValue(0);
            builder.Property(table => table.OrderId).HasColumnName("order_id").HasDefaultValue(0);
            builder.Property(table => table.UsedAt).HasColumnName("used_at");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
