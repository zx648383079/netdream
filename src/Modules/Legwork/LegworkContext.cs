using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Legwork.Entities;
using NetDream.Modules.Legwork.Migrations;

namespace NetDream.Modules.Legwork;
public class LegworkContext(DbContextOptions<LegworkContext> options) : DbContext(options)
{
    public DbSet<CategoryEntity> Categories { get; set; }
    public DbSet<CategoryProviderEntity> CategoryProvider { get; set; }
    public DbSet<OrderEntity> Order { get; set; }
    public DbSet<OrderLogEntity> OrderLog { get; set; }
    public DbSet<ProviderEntity> Provider { get; set; }
    public DbSet<ServiceEntity> Service { get; set; }
    public DbSet<ServiceRegionEntity> ServiceRegion { get; set; }
    public DbSet<ServiceWaiterEntity> ServiceWaiter { get; set; }
    public DbSet<WaiterEntity> Waiter { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CategoryEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryProviderEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OrderEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OrderLogEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ProviderEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ServiceEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ServiceRegionEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ServiceWaiterEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new WaiterEntityTypeConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}