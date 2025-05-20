using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Finance.Entities;

namespace NetDream.Modules.Finance.Migrations;
public class MoneyAccountEntityTypeConfiguration : IEntityTypeConfiguration<MoneyAccountEntity>
{
    public void Configure(EntityTypeBuilder<MoneyAccountEntity> builder)
    {
        builder.ToTable("finance_money_account", table => table.HasComment("资金账户"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(35).HasComment("账户名");
        builder.Property(table => table.Money).HasColumnName("money").HasMaxLength(10).HasDefaultValue(0.00).HasComment("可用金额");
        builder.Property(table => table.FrozenMoney).HasColumnName("frozen_money").HasMaxLength(10).HasDefaultValue(0.00).HasComment("冻结金额");
        builder.Property(table => table.Status).HasColumnName("status").HasDefaultValue(1).HasComment("1 正常 0 删除");
        builder.Property(table => table.Remark).HasColumnName("remark").HasComment("备注");
        builder.Property(table => table.UserId).HasColumnName("user_id").HasMaxLength(10);
        builder.Property(table => table.DeletedAt).HasColumnName("deleted_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.UpdatedAt).HasColumnName("updated_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasMaxLength(10).HasDefaultValue(0);
        
    }
}