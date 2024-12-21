using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Migrations
{
    public class WarehouseLogEntityTypeConfiguration : IEntityTypeConfiguration<WarehouseLogEntity>
    {
        public void Configure(EntityTypeBuilder<WarehouseLogEntity> builder)
        {
            builder.ToTable("WarehouseLog", table => table.HasComment(""));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.WarehouseId).HasColumnName("warehouse_id");
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.GoodsId).HasColumnName("goods_id");
            builder.Property(table => table.ProductId).HasColumnName("product_id").HasDefaultValue(0);
            builder.Property(table => table.Amount).HasColumnName("amount");
            builder.Property(table => table.OrderId).HasColumnName("order_id").HasDefaultValue(0);
            builder.Property(table => table.Remark).HasColumnName("remark").HasDefaultValue(string.Empty);
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
