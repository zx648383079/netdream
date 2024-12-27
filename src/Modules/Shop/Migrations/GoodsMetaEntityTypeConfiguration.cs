using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Migrations
{
    public class GoodsMetaEntityTypeConfiguration : IEntityTypeConfiguration<GoodsMetaEntity>
    {
        public void Configure(EntityTypeBuilder<GoodsMetaEntity> builder)
        {
            builder.ToTable("GoodsMeta", table => table.HasComment(""));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.GoodsId).HasColumnName("goods_id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(50);
            builder.Property(table => table.Content).HasColumnName("content");
        }
    }
}
