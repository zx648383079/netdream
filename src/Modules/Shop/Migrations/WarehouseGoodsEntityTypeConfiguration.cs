using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Migrations
{
    public class WarehouseGoodsEntityTypeConfiguration : IEntityTypeConfiguration<WarehouseGoodsEntity>
    {
        public void Configure(EntityTypeBuilder<WarehouseGoodsEntity> builder)
        {
            builder.ToTable("WarehouseGoods", table => table.HasComment(""));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.WarehouseId).HasColumnName("warehouse_id");
            builder.Property(table => table.GoodsId).HasColumnName("goods_id");
            builder.Property(table => table.ProductId).HasColumnName("product_id").HasDefaultValue(0);
            builder.Property(table => table.Amount).HasColumnName("amount").HasDefaultValue(0);
        }
    }
}
