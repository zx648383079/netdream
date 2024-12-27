using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Migrations
{
    public class CouponEntityTypeConfiguration : IEntityTypeConfiguration<CouponEntity>
    {
        public void Configure(EntityTypeBuilder<CouponEntity> builder)
        {
            builder.ToTable("Coupon", table => table.HasComment("优惠券"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(30);
            builder.Property(table => table.Thumb).HasColumnName("thumb").HasDefaultValue(string.Empty);
            builder.Property(table => table.Type).HasColumnName("type").HasMaxLength(2).HasDefaultValue(0).HasComment("优惠类型");
            builder.Property(table => table.Rule).HasColumnName("rule").HasMaxLength(2).HasDefaultValue(0).HasComment("使用的商品");
            builder.Property(table => table.RuleValue).HasColumnName("rule_value").HasDefaultValue(string.Empty).HasComment("使用的商品");
            builder.Property(table => table.MinMoney).HasColumnName("min_money").HasMaxLength(8).HasDefaultValue(0).HasComment("满多少可用");
            builder.Property(table => table.Money).HasColumnName("money").HasMaxLength(8).HasDefaultValue(0).HasComment("几折或优惠金额");
            builder.Property(table => table.SendType).HasColumnName("send_type").HasDefaultValue(0).HasComment("发放类型");
            builder.Property(table => table.SendValue).HasColumnName("send_value").HasDefaultValue(0).HasComment("发放条件或数量");
            builder.Property(table => table.EveryAmount).HasColumnName("every_amount").HasDefaultValue(0).HasComment("每个用户能领取数量");
            builder.Property(table => table.StartAt).HasColumnName("start_at").HasComment("有效期开始时间");
            builder.Property(table => table.EndAt).HasColumnName("end_at").HasComment("有效期结束时间");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
