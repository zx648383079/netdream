using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Migrations
{
    public class DeliveryGoodsEntityTypeConfiguration : IEntityTypeConfiguration<DeliveryGoodsEntity>
    {
        public void Configure(EntityTypeBuilder<DeliveryGoodsEntity> builder)
        {
            builder.ToTable("DeliveryGoods", table => table.HasComment(""));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.DeliveryId).HasColumnName("delivery_id");
            builder.Property(table => table.OrderGoodsId).HasColumnName("order_goods_id");
            builder.Property(table => table.GoodsId).HasColumnName("goods_id");
            builder.Property(table => table.ProductId).HasColumnName("product_id").HasDefaultValue(0);
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(100).HasComment("商品名");
            builder.Property(table => table.Thumb).HasColumnName("thumb");
            builder.Property(table => table.SeriesNumber).HasColumnName("series_number").HasMaxLength(100);
            builder.Property(table => table.Amount).HasColumnName("amount").HasDefaultValue(1);
            builder.Property(table => table.TypeRemark).HasColumnName("type_remark").HasDefaultValue(string.Empty).HasComment("商品类型备注信息");
        }
    }
}
