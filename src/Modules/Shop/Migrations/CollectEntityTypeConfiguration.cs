using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Migrations
{
    public class CollectEntityTypeConfiguration : IEntityTypeConfiguration<CollectEntity>
    {
        public void Configure(EntityTypeBuilder<CollectEntity> builder)
        {
            builder.ToTable("Collect", table => table.HasComment(""));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.GoodsId).HasColumnName("goods_id");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
