using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Plan.Entities;
using NetDream.Modules.Plan.Migrations;

namespace NetDream.Modules.Plan;
public class PlanContext(DbContextOptions<PlanContext> options) : DbContext(options)
{
    public DbSet<CommentEntity> Comment { get; set; }
    public DbSet<DayEntity> Day { get; set; }
    public DbSet<LogEntity> Log { get; set; }
    public DbSet<ShareEntity> Share { get; set; }
    public DbSet<ShareUserEntity> ShareUser { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CommentEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new DayEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new LogEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ShareEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ShareUserEntityTypeConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}