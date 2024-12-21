using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Migrations
{
    public class PayLogEntityTypeConfiguration : IEntityTypeConfiguration<PayLogEntity>
    {
        public void Configure(EntityTypeBuilder<PayLogEntity> builder)
        {
            builder.ToTable("PayLog", table => table.HasComment(""));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.PaymentId).HasColumnName("payment_id");
            builder.Property(table => table.PaymentName).HasColumnName("payment_name").HasMaxLength(30).HasDefaultValue(string.Empty);
            builder.Property(table => table.Type).HasColumnName("type").HasMaxLength(1).HasDefaultValue(0);
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.Data).HasColumnName("data").HasDefaultValue(string.Empty).HasComment("可以接受多个订单号");
            builder.Property(table => table.Status).HasColumnName("status").HasMaxLength(2).HasDefaultValue(0);
            builder.Property(table => table.Amount).HasColumnName("amount").HasMaxLength(10).HasDefaultValue(0);
            builder.Property(table => table.Currency).HasColumnName("currency").HasMaxLength(10).HasDefaultValue(string.Empty).HasComment("货币");
            builder.Property(table => table.CurrencyMoney).HasColumnName("currency_money").HasMaxLength(10).HasDefaultValue(0).HasComment("货币金额");
            builder.Property(table => table.TradeNo).HasColumnName("trade_no").HasMaxLength(100).HasDefaultValue(string.Empty).HasComment("外部订单号");
            builder.Property(table => table.BeginAt).HasColumnName("begin_at").HasComment("开始时间");
            builder.Property(table => table.ConfirmAt).HasColumnName("confirm_at").HasComment("确认支付时间");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
