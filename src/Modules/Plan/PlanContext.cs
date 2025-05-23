using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Plan.Entities;
using NetDream.Modules.Plan.Migrations;

namespace NetDream.Modules.Plan;
public class PlanContext(DbContextOptions<PlanContext> options) : DbContext(options)
{
    public DbSet<CommentEntity> Comments { get; set; }
    public DbSet<DayEntity> Days { get; set; }
    public DbSet<TaskEntity> Tasks { get; set; }
    public DbSet<LogEntity> Logs { get; set; }
    public DbSet<ShareEntity> Shares { get; set; }
    public DbSet<PlanEntity> Plans { get; set; }
    public DbSet<ShareUserEntity> ShareUsers { get; set; }
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