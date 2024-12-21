using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Trade.Entities;

namespace NetDream.Modules.Trade.Migrations
{
    public class RefundEntityTypeConfiguration : IEntityTypeConfiguration<RefundEntity>
    {
        public void Configure(EntityTypeBuilder<RefundEntity> builder)
        {
            builder.ToTable("trade_refund", table => table.HasComment("支付退款系统"));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.TradeId).HasColumnName("trade_id");
            builder.Property(table => table.OutRequestNo).HasColumnName("out_request_no").HasMaxLength(64).HasComment("标识一次退款请求，同一笔交易多次退款需要保证唯一，如需部分退款，则此参数必传");
            builder.Property(table => table.RefundReason).HasColumnName("refund_reason").HasDefaultValue(string.Empty).HasComment("退款的原因说明");
            builder.Property(table => table.RefundAmount).HasColumnName("refund_amount").HasMaxLength(10).HasComment("需要退款的金额");
            builder.Property(table => table.OperatorId).HasColumnName("operator_id").HasMaxLength(28).HasDefaultValue(string.Empty).HasComment("商户操作员编号");
            builder.Property(table => table.Status).HasColumnName("status").HasMaxLength(2).HasDefaultValue(0);
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
