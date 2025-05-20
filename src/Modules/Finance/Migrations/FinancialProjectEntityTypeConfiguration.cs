using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Finance.Entities;

namespace NetDream.Modules.Finance.Migrations;
public class FinancialProjectEntityTypeConfiguration : IEntityTypeConfiguration<FinancialProjectEntity>
{
    public void Configure(EntityTypeBuilder<FinancialProjectEntity> builder)
    {
        builder.ToTable("finance_financial_project", table => table.HasComment("理财项目"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(35).HasComment("配置项目");
        builder.Property(table => table.Alias).HasColumnName("alias").HasMaxLength(50).HasComment("别名");
        builder.Property(table => table.Money).HasColumnName("money").HasMaxLength(10).HasDefaultValue(0.00).HasComment("金额");
        builder.Property(table => table.AccountId).HasColumnName("account_id").HasMaxLength(10).HasComment("账户");
        builder.Property(table => table.Earnings).HasColumnName("earnings").HasMaxLength(10).HasComment("收益率/预估收益率");
        builder.Property(table => table.StartAt).HasColumnName("start_at").HasComment("起息日期");
        builder.Property(table => table.EndAt).HasColumnName("end_at").HasComment("到期日期");
        builder.Property(table => table.EarningsNumber).HasColumnName("earnings_number").HasMaxLength(10).HasComment("到期收益");
        builder.Property(table => table.ProductId).HasColumnName("product_id").HasMaxLength(10).HasComment("理财产品");
        builder.Property(table => table.Status).HasColumnName("status").HasDefaultValue(1).HasComment("0 已结束  1 进行中");
        builder.Property(table => table.Color).HasColumnName("color").HasDefaultValue(1).HasComment("1 红色（赚） 0 绿（亏）");
        builder.Property(table => table.Remark).HasColumnName("remark").HasComment("备注");
        builder.Property(table => table.UserId).HasColumnName("user_id").HasMaxLength(10);
        builder.Property(table => table.DeletedAt).HasColumnName("deleted_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.UpdatedAt).HasColumnName("updated_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasMaxLength(10).HasDefaultValue(0);
        
    }
}