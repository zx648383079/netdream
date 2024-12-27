using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Migrations
{
    public class GoodsCardEntityTypeConfiguration : IEntityTypeConfiguration<GoodsCardEntity>
    {
        public void Configure(EntityTypeBuilder<GoodsCardEntity> builder)
        {
            builder.ToTable("GoodsCard", table => table.HasComment(""));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.GoodsId).HasColumnName("goods_id");
            builder.Property(table => table.CardNo).HasColumnName("card_no");
            builder.Property(table => table.CardPwd).HasColumnName("card_pwd");
            builder.Property(table => table.OrderId).HasColumnName("order_id").HasDefaultValue(0);
        }
    }
}
