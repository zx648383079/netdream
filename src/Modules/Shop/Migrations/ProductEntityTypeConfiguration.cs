using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Migrations
{
    public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<ProductEntity>
    {
        public void Configure(EntityTypeBuilder<ProductEntity> builder)
        {
            builder.ToTable("Product", table => table.HasComment(""));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.GoodsId).HasColumnName("goods_id");
            builder.Property(table => table.Price).HasColumnName("price").HasMaxLength(10).HasDefaultValue(0);
            builder.Property(table => table.MarketPrice).HasColumnName("market_price").HasMaxLength(10).HasDefaultValue(0);
            builder.Property(table => table.Stock).HasColumnName("stock").HasDefaultValue(1);
            builder.HasDefaultValue(0);
            builder.Property(table => table.SeriesNumber).HasColumnName("series_number").HasMaxLength(50).HasDefaultValue(string.Empty);
            builder.Property(table => table.Attributes).HasColumnName("attributes").HasMaxLength(100).HasDefaultValue(string.Empty);
        }
    }
}
