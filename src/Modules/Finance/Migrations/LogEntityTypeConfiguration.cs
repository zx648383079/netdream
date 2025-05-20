using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Finance.Entities;

namespace NetDream.Modules.Finance.Migrations;
public class LogEntityTypeConfiguration : IEntityTypeConfiguration<LogEntity>
{
    public void Configure(EntityTypeBuilder<LogEntity> builder)
    {
        builder.ToTable("finance_log", table => table.HasComment("资金变动记录"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.ParentId).HasColumnName("parent_id").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.Type).HasColumnName("type").HasDefaultValue(1).HasComment("1 收入  0 支出 2 借出 3 借入");
        builder.Property(table => table.Money).HasColumnName("money").HasMaxLength(10).HasDefaultValue(0.00).HasComment("金额");
        builder.Property(table => table.FrozenMoney).HasColumnName("frozen_money").HasMaxLength(10).HasDefaultValue(0.00).HasComment("冻结金额");
        builder.Property(table => table.AccountId).HasColumnName("account_id").HasMaxLength(10).HasComment("资金账户");
        builder.Property(table => table.ChannelId).HasColumnName("channel_id").HasMaxLength(10).HasDefaultValue(0).HasComment("支出时填写消费渠道");
        builder.Property(table => table.ProjectId).HasColumnName("project_id").HasMaxLength(10).HasDefaultValue(0).HasComment("收入时填写理财项目");
        builder.Property(table => table.BudgetId).HasColumnName("budget_id").HasMaxLength(10).HasDefaultValue(0).HasComment("支出时选择预算");
        builder.Property(table => table.Remark).HasColumnName("remark").HasComment("备注");
        builder.Property(table => table.HappenedAt).HasColumnName("happened_at").HasComment("发生时间");
        builder.Property(table => table.OutTradeNo).HasColumnName("out_trade_no").HasMaxLength(100).HasComment("外部导入的交易单号");
        builder.Property(table => table.UserId).HasColumnName("user_id").HasMaxLength(10);
        builder.Property(table => table.TradingObject).HasColumnName("trading_object").HasMaxLength(100).HasComment("交易对象");
        builder.Property(table => table.UpdatedAt).HasColumnName("updated_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasMaxLength(10).HasDefaultValue(0);
        
    }
}