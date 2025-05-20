using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Finance.Entities;
using NetDream.Modules.Finance.Migrations;

namespace NetDream.Modules.Finance;
public class FinanceContext(DbContextOptions<FinanceContext> options) : DbContext(options)
{
    public DbSet<BudgetEntity> Budget { get; set; }
    public DbSet<ConsumptionChannelEntity> ConsumptionChannel { get; set; }
    public DbSet<FinancialProductEntity> FinancialProduct { get; set; }
    public DbSet<FinancialProjectEntity> FinancialProject { get; set; }
    public DbSet<LogEntity> Log { get; set; }
    public DbSet<MoneyAccountEntity> MoneyAccount { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new BudgetEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ConsumptionChannelEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new FinancialProductEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new FinancialProjectEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new LogEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new MoneyAccountEntityTypeConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}