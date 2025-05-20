using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Finance.Entities;

namespace NetDream.Modules.Finance.Migrations;
public class BudgetEntityTypeConfiguration : IEntityTypeConfiguration<BudgetEntity>
{
    public void Configure(EntityTypeBuilder<BudgetEntity> builder)
    {
        builder.ToTable("finance_budget", table => table.HasComment("预算计划"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(50).HasComment("预算名");
        builder.Property(table => table.Budget).HasColumnName("budget").HasMaxLength(10).HasDefaultValue(0.00).HasComment("预算");
        builder.Property(table => table.Spent).HasColumnName("spent").HasMaxLength(10).HasDefaultValue(0.00).HasComment("花费");
        builder.Property(table => table.Cycle).HasColumnName("cycle").HasDefaultValue(0).HasComment("周期");
        builder.Property(table => table.UserId).HasColumnName("user_id").HasMaxLength(10);
        builder.Property(table => table.DeletedAt).HasColumnName("deleted_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.UpdatedAt).HasColumnName("updated_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasMaxLength(10).HasDefaultValue(0);
        
    }
}