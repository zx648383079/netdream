using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Catering.Entities;

namespace NetDream.Modules.Catering.Migrations;
public class OrderGoodsEntityTypeConfiguration : IEntityTypeConfiguration<OrderGoodsEntity>
{
    public void Configure(EntityTypeBuilder<OrderGoodsEntity> builder)
    {
        builder.ToTable("eat_order_goods", table => table.HasComment(""));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.OrderId).HasColumnName("order_id").HasMaxLength(10);
        builder.Property(table => table.GoodsId).HasColumnName("goods_id").HasMaxLength(10);
        builder.Property(table => table.Amount).HasColumnName("amount").HasMaxLength(10);
        builder.Property(table => table.Price).HasColumnName("price").HasMaxLength(8);
        builder.Property(table => table.Discount).HasColumnName("discount").HasMaxLength(8);
        builder.Property(table => table.Status).HasColumnName("status").HasDefaultValue(0);
        builder.Property(table => table.UpdatedAt).HasColumnName("updated_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasMaxLength(10).HasDefaultValue(0);
        
    }
}