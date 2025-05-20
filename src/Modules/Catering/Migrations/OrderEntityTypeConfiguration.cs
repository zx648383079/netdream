using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Catering.Entities;

namespace NetDream.Modules.Catering.Migrations;
public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<OrderEntity>
{
    public void Configure(EntityTypeBuilder<OrderEntity> builder)
    {
        builder.ToTable("eat_order", table => table.HasComment(""));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.UserId).HasColumnName("user_id").HasMaxLength(10);
        builder.Property(table => table.StoreId).HasColumnName("store_id").HasMaxLength(10);
        builder.Property(table => table.WaiterId).HasColumnName("waiter_id").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.AddressType).HasColumnName("address_type").HasDefaultValue(0);
        builder.Property(table => table.AddressName).HasColumnName("address_name").HasMaxLength(20);
        builder.Property(table => table.AddressTel).HasColumnName("address_tel").HasMaxLength(20);
        builder.Property(table => table.Address).HasColumnName("address").HasMaxLength(255);
        builder.Property(table => table.PaymentId).HasColumnName("payment_id").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.PaymentName).HasColumnName("payment_name").HasMaxLength(30).HasDefaultValue("0");
        builder.Property(table => table.GoodsAmount).HasColumnName("goods_amount").HasMaxLength(8).HasDefaultValue(0.00);
        builder.Property(table => table.OrderAmount).HasColumnName("order_amount").HasMaxLength(8).HasDefaultValue(0.00);
        builder.Property(table => table.Discount).HasColumnName("discount").HasMaxLength(8).HasDefaultValue(0.00);
        builder.Property(table => table.ShippingFee).HasColumnName("shipping_fee").HasMaxLength(8).HasDefaultValue(0.00);
        builder.Property(table => table.PayFee).HasColumnName("pay_fee").HasMaxLength(8).HasDefaultValue(0.00);
        builder.Property(table => table.Status).HasColumnName("status").HasDefaultValue(0);
        builder.Property(table => table.Remark).HasColumnName("remark").HasMaxLength(255);
        builder.Property(table => table.ReserveAt).HasColumnName("reserve_at").HasMaxLength(255).HasComment("预约时间");
        builder.Property(table => table.PayAt).HasColumnName("pay_at").HasMaxLength(10).HasDefaultValue(0).HasComment("支付时间");
        builder.Property(table => table.ShippingAt).HasColumnName("shipping_at").HasMaxLength(10).HasDefaultValue(0).HasComment("发货时间");
        builder.Property(table => table.ReceiveAt).HasColumnName("receive_at").HasMaxLength(10).HasDefaultValue(0).HasComment("签收时间");
        builder.Property(table => table.FinishAt).HasColumnName("finish_at").HasMaxLength(10).HasDefaultValue(0).HasComment("完成时间");
        builder.Property(table => table.UpdatedAt).HasColumnName("updated_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasMaxLength(10).HasDefaultValue(0);
        
    }
}