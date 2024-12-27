using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Migrations
{
    public class CartEntityTypeConfiguration : IEntityTypeConfiguration<CartEntity>
    {
        public void Configure(EntityTypeBuilder<CartEntity> builder)
        {
            builder.ToTable("Cart", table => table.HasComment(""));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Type).HasColumnName("type").HasMaxLength(1).HasDefaultValue(0);
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.GoodsId).HasColumnName("goods_id");
            builder.Property(table => table.ProductId).HasColumnName("product_id").HasDefaultValue(0);
            builder.Property(table => table.Amount).HasColumnName("amount").HasDefaultValue(1);
            builder.Property(table => table.Price).HasColumnName("price").HasMaxLength(8);
            builder.Property(table => table.IsChecked).HasColumnName("is_checked").HasDefaultValue(0).HasComment("是否选中");
            builder.Property(table => table.SelectedActivity).HasColumnName("selected_activity").HasDefaultValue(0).HasComment("选择的活动");
            builder.Property(table => table.AttributeId).HasColumnName("attribute_id").HasDefaultValue(string.Empty).HasComment("选择的属性");
            builder.Property(table => table.AttributeValue).HasColumnName("attribute_value").HasDefaultValue(string.Empty).HasComment("选择的属性值");
            builder.Property(table => table.ExpiredAt).HasColumnName("expired_at").HasComment("过期时间");
        }
    }
}
