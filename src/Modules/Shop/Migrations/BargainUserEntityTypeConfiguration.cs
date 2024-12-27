using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Migrations
{
    public class BargainUserEntityTypeConfiguration : IEntityTypeConfiguration<BargainUserEntity>
    {
        public void Configure(EntityTypeBuilder<BargainUserEntity> builder)
        {
            builder.ToTable("BargainUser", table => table.HasComment("用户参与砍价"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.ActId).HasColumnName("act_id");
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.GoodsId).HasColumnName("goods_id");
            builder.Property(table => table.Price).HasColumnName("price").HasMaxLength(10).HasComment("当前价格");
            builder.Property(table => table.Status).HasColumnName("status").HasMaxLength(1).HasDefaultValue(0);
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
