using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Trade.Entities;

namespace NetDream.Modules.Trade.Migrations
{
    internal class TradeEntityTypeConfiguration : IEntityTypeConfiguration<TradeEntity>
    {
        public void Configure(EntityTypeBuilder<TradeEntity> builder)
        {
            builder.ToTable("trade", table => table.HasComment("支付系统"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.OpenId).HasColumnName("open_id");
            builder.Property(table => table.BuyerId).HasColumnName("buyer_id").HasComment("买家");
            builder.Property(table => table.SellerId).HasColumnName("seller_id").HasComment("收款方");
            builder.Property(table => table.OutTradeNo).HasColumnName("out_trade_no").HasMaxLength(64).HasComment("商户订单号,64个字符以内、可包含字母、数字、下划线；需保证在商户端不重复");
            builder.Property(table => table.Subject).HasColumnName("subject");
            builder.Property(table => table.Body).HasColumnName("body").HasDefaultValue(string.Empty).HasComment("订单描述");
            builder.Property(table => table.TotalAmount).HasColumnName("total_amount").HasMaxLength(10).HasComment("订单总金额");
            builder.Property(table => table.OperatorId).HasColumnName("operator_id").HasMaxLength(28).HasDefaultValue(string.Empty).HasComment("商户操作员编号");
            builder.Property(table => table.TimeoutExpress).HasColumnName("timeout_express").HasMaxLength(6).HasDefaultValue(string.Empty).HasComment("该笔订单允许的最晚付款时间，逾期将关闭交易");
            builder.Property(table => table.NotifyUrl).HasColumnName("notify_url").HasDefaultValue(string.Empty).HasComment("通知地址");
            builder.Property(table => table.ReturnUrl).HasColumnName("return_url").HasDefaultValue(string.Empty).HasComment("返回地址");
            builder.Property(table => table.PassbackParams).HasColumnName("passback_params").HasMaxLength(512).HasDefaultValue(string.Empty).HasComment("公用回传参数，如果请求时传递了该参数，则返回给商户时会回传该参数。支付宝只会在同步返回（包括跳转回商户网站）和异步通知时将该参数原样返回");
            builder.Property(table => table.Status).HasColumnName("status").HasMaxLength(2).HasDefaultValue(0);
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
