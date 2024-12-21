using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Migrations
{
    public class GoodsAttributeEntityTypeConfiguration : IEntityTypeConfiguration<GoodsAttributeEntity>
    {
        public void Configure(EntityTypeBuilder<GoodsAttributeEntity> builder)
        {
            builder.ToTable("GoodsAttribute", table => table.HasComment(""));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.GoodsId).HasColumnName("goods_id").HasDefaultValue(0);
            builder.Property(table => table.AttributeId).HasColumnName("attribute_id");
            builder.Property(table => table.Value).HasColumnName("value");
            builder.Property(table => table.Price).HasColumnName("price").HasMaxLength(10).HasDefaultValue(0).HasComment("附加服务，多选不算在");
        }
    }
}
