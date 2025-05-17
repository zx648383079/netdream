using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.TradeTracker.Entities;
using System;

namespace NetDream.Modules.TradeTracker.Migrations
{
    public class TradeLogEntityTypeConfiguration : IEntityTypeConfiguration<TradeLogEntity>
    {
        public void Configure(EntityTypeBuilder<TradeLogEntity> builder)
        {
            builder.ToTable("tt_trade_log", table => table.HasComment("价格变动记录（只保留一天）"));
            builder.HasKey(table => table.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.ProductId).HasColumnName("product_id");
            builder.Property(table => table.ChannelId).HasColumnName("channel_id");
            builder.Property(table => table.Type).HasColumnName("type").HasDefaultValue(0).HasComment("0 出售, 1 求购");
            builder.Property(table => table.Price).HasColumnName("price").HasMaxLength(12);
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
