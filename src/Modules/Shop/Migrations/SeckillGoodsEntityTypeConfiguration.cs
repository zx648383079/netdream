using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Migrations
{
    public class SeckillGoodsEntityTypeConfiguration : IEntityTypeConfiguration<SeckillGoodsEntity>
    {
        public void Configure(EntityTypeBuilder<SeckillGoodsEntity> builder)
        {
            builder.ToTable("SeckillGoods", table => table.HasComment(""));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.ActId).HasColumnName("act_id");
            builder.Property(table => table.TimeId).HasColumnName("time_id");
            builder.Property(table => table.GoodsId).HasColumnName("goods_id");
            builder.Property(table => table.Price).HasColumnName("price").HasMaxLength(8).HasDefaultValue(0);
            builder.Property(table => table.Amount).HasColumnName("amount").HasMaxLength(5).HasDefaultValue(0);
            builder.Property(table => table.EveryAmount).HasColumnName("every_amount").HasMaxLength(2).HasDefaultValue(0);
        }
    }
}
