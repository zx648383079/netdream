using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.TradeTracker.Entities;

namespace NetDream.Modules.TradeTracker.Migrations
{
    public class TradeEntityTypeConfiguration : IEntityTypeConfiguration<TradeEntity>
    {
        public void Configure(EntityTypeBuilder<TradeEntity> builder)
        {
            builder.ToTable("tt_trades", table => table.HasComment("交易价格（按天取出售最低价格求购最高价）"));
            builder.HasKey(table => table.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.ProductId).HasColumnName("product_id");
            builder.Property(table => table.ChannelId).HasColumnName("channel_id");
            builder.Property(table => table.Type).HasColumnName("type").HasDefaultValue(0).HasComment("0 出售, 1 求购");
            builder.Property(table => table.Price).HasColumnName("price").HasMaxLength(12);
            builder.Property(table => table.OrderCount).HasColumnName("order_count").HasDefaultValue(0);
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
