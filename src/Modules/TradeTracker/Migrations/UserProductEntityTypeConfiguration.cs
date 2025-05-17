using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.TradeTracker.Entities;

namespace NetDream.Modules.TradeTracker.Migrations
{
    public class UserProductEntityTypeConfiguration : IEntityTypeConfiguration<UserProductEntity>
    {
        public void Configure(EntityTypeBuilder<UserProductEntity> builder)
        {
            builder.ToTable("tt_user_product", table => table.HasComment("已购商品"));
            builder.HasKey(table => table.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.ProductId).HasColumnName("product_id");
            builder.Property(table => table.ChannelId).HasColumnName("channel_id");
            builder.Property(table => table.Price).HasColumnName("price").HasMaxLength(12);
            builder.Property(table => table.SellPrice).HasColumnName("sell_price").HasMaxLength(12).HasDefaultValue(0);
            builder.Property(table => table.SellChannelId).HasColumnName("sell_channel_id").HasDefaultValue(0);
            builder.Property(table => table.Status).HasColumnName("status").HasDefaultValue(0);
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
