using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Migrations
{
    public class PresaleLogEntityTypeConfiguration : IEntityTypeConfiguration<PresaleLogEntity>
    {
        public void Configure(EntityTypeBuilder<PresaleLogEntity> builder)
        {
            builder.ToTable("PresaleLog", table => table.HasComment("预售记录"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.ActId).HasColumnName("act_id");
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.OrderId).HasColumnName("order_id");
            builder.Property(table => table.OrderGoodsId).HasColumnName("order_goods_id");
            builder.Property(table => table.OrderAmount).HasColumnName("order_amount").HasMaxLength(8)
                .HasDefaultValue(0).HasComment("预售总价");
            builder.Property(table => table.Deposit).HasColumnName("deposit").HasMaxLength(8)
                .HasDefaultValue(0).HasComment("预售定金");
            builder.Property(table => table.FinalPayment).HasColumnName("final_payment").HasMaxLength(8)
                .HasDefaultValue(0).HasComment("预售尾款");
            builder.Property(table => table.Status).HasColumnName("status").HasMaxLength(2).HasDefaultValue(0).HasComment("判断预售订单处于那个状态");
            builder.Property(table => table.PredeterminedAt).HasColumnName("predetermined_at").HasComment("支付定金时间");
            builder.Property(table => table.FinalAt).HasColumnName("final_at").HasComment("尾款支付时间");
            builder.Property(table => table.ShipAt).HasColumnName("ship_at").HasComment("发货时间");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
