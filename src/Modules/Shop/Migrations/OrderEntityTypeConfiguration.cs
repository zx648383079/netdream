using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Migrations
{
    public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<OrderEntity>
    {
        public void Configure(EntityTypeBuilder<OrderEntity> builder)
        {
            builder.ToTable("Order", table => table.HasComment(""));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.SeriesNumber).HasColumnName("series_number").HasMaxLength(100);
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.Status).HasColumnName("status").HasDefaultValue(0);
            builder.Property(table => table.PaymentId).HasColumnName("payment_id").HasDefaultValue(string.Empty).HasComment("支付 code");
            builder.Property(table => table.PaymentName).HasColumnName("payment_name").HasMaxLength(30).HasDefaultValue(0);
            builder.Property(table => table.ShippingId).HasColumnName("shipping_id").HasDefaultValue(string.Empty).HasComment("物流 code");
            builder.Property(table => table.InvoiceId).HasColumnName("invoice_id").HasDefaultValue(0).HasComment("发票");
            builder.Property(table => table.ShippingName).HasColumnName("shipping_name").HasMaxLength(30).HasDefaultValue(0);
            builder.Property(table => table.GoodsAmount).HasColumnName("goods_amount").HasMaxLength(8).HasDefaultValue(0);
            builder.Property(table => table.OrderAmount).HasColumnName("order_amount").HasMaxLength(8).HasDefaultValue(0);
            builder.Property(table => table.Discount).HasColumnName("discount").HasMaxLength(8).HasDefaultValue(0);
            builder.Property(table => table.ShippingFee).HasColumnName("shipping_fee").HasMaxLength(8).HasDefaultValue(0);
            builder.Property(table => table.PayFee).HasColumnName("pay_fee").HasMaxLength(8).HasDefaultValue(0);
            builder.Property(table => table.ReferenceType).HasColumnName("reference_type").HasMaxLength(1).HasDefaultValue(0).HasComment("来源类型");
            builder.Property(table => table.ReferenceId).HasColumnName("reference_id").HasDefaultValue(0).HasComment("来源相关id");
            builder.Property(table => table.PayAt).HasColumnName("pay_at").HasComment("支付时间");
            builder.Property(table => table.ShippingAt).HasColumnName("shipping_at").HasComment("发货时间");
            builder.Property(table => table.ReceiveAt).HasColumnName("receive_at").HasComment("签收时间");
            builder.Property(table => table.FinishAt).HasColumnName("finish_at").HasComment("完成时间");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
