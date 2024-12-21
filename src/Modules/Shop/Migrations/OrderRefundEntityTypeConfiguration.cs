using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Migrations
{
    public class OrderRefundEntityTypeConfiguration : IEntityTypeConfiguration<OrderRefundEntity>
    {
        public void Configure(EntityTypeBuilder<OrderRefundEntity> builder)
        {
            builder.ToTable("OrderRefund", table => table.HasComment("订单售后服务"));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.OrderId).HasColumnName("order_id");
            builder.Property(table => table.OrderGoodsId).HasColumnName("order_goods_id").HasDefaultValue(0);
            builder.Property(table => table.GoodsId).HasColumnName("goods_id");
            builder.Property(table => table.ProductId).HasColumnName("product_id").HasDefaultValue(0);
            builder.Property(table => table.Title).HasColumnName("title");
            builder.Property(table => table.Amount).HasColumnName("amount").HasDefaultValue(1).HasComment("数量");
            builder.Property(table => table.Type).HasColumnName("type").HasMaxLength(2).HasDefaultValue(0);
            builder.Property(table => table.Status).HasColumnName("status").HasMaxLength(2).HasDefaultValue(0);
            builder.Property(table => table.Reason).HasColumnName("reason").HasDefaultValue(string.Empty);
            builder.Property(table => table.Description).HasColumnName("description").HasDefaultValue(string.Empty);
            builder.Property(table => table.Evidence).HasColumnName("evidence").HasDefaultValue(string.Empty).HasComment("证据,json格式");
            builder.Property(table => table.Explanation).HasColumnName("explanation").HasDefaultValue(string.Empty).HasComment("平台回复");
            builder.Property(table => table.Money).HasColumnName("money").HasMaxLength(10).HasDefaultValue(0);
            builder.Property(table => table.OrderPrice).HasColumnName("order_price").HasMaxLength(10).HasDefaultValue(0);
            builder.Property(table => table.Freight).HasColumnName("freight").HasMaxLength(2)
                .HasDefaultValue(0).HasComment("退款方式");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
