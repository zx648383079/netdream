using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Legwork.Entities;

namespace NetDream.Modules.Legwork.Migrations;
public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<OrderEntity>
{
    public void Configure(EntityTypeBuilder<OrderEntity> builder)
    {
        builder.ToTable("leg_order", table => table.HasComment("跑腿订单"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.ProviderId).HasColumnName("provider_id").HasMaxLength(10);
        builder.Property(table => table.UserId).HasColumnName("user_id").HasMaxLength(10);
        builder.Property(table => table.ServiceId).HasColumnName("service_id").HasMaxLength(10);
        builder.Property(table => table.Amount).HasColumnName("amount").HasMaxLength(5).HasDefaultValue(1).HasComment("购买服务的数量");
        builder.Property(table => table.Remark).HasColumnName("remark").HasComment("服务内容");
        builder.Property(table => table.OrderAmount).HasColumnName("order_amount").HasMaxLength(8).HasDefaultValue(0.00).HasComment("订单金额");
        builder.Property(table => table.WaiterId).HasColumnName("waiter_id").HasMaxLength(10).HasDefaultValue(0).HasComment("跑腿人");
        builder.Property(table => table.Status).HasColumnName("status").HasDefaultValue(10);
        builder.Property(table => table.ServiceScore).HasColumnName("service_score").HasDefaultValue(10).HasComment("服务评分");
        builder.Property(table => table.WaiterScore).HasColumnName("waiter_score").HasDefaultValue(10).HasComment("服务员评分");
        builder.Property(table => table.PayAt).HasColumnName("pay_at").HasMaxLength(10).HasDefaultValue(0).HasComment("支付时间");
        builder.Property(table => table.TakingAt).HasColumnName("taking_at").HasMaxLength(10).HasDefaultValue(0).HasComment("接单时间");
        builder.Property(table => table.TakenAt).HasColumnName("taken_at").HasMaxLength(10).HasDefaultValue(0).HasComment("完成接单任务时间");
        builder.Property(table => table.FinishAt).HasColumnName("finish_at").HasMaxLength(10).HasDefaultValue(0).HasComment("完成时间");
        builder.Property(table => table.UpdatedAt).HasColumnName("updated_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasMaxLength(10).HasDefaultValue(0);
        
    }
}