using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Finance.Entities;

namespace NetDream.Modules.Finance.Migrations;
public class FinancialProductEntityTypeConfiguration : IEntityTypeConfiguration<FinancialProductEntity>
{
    public void Configure(EntityTypeBuilder<FinancialProductEntity> builder)
    {
        builder.ToTable("finance_financial_product", table => table.HasComment("理财产品"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(50);
        builder.Property(table => table.Status).HasColumnName("status").HasDefaultValue(1).HasComment("1 正常 0 删除");
        builder.Property(table => table.Remark).HasColumnName("remark").HasComment("备注");
        builder.Property(table => table.UserId).HasColumnName("user_id").HasMaxLength(10);
        builder.Property(table => table.UpdatedAt).HasColumnName("updated_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasMaxLength(10).HasDefaultValue(0);
        
    }
}