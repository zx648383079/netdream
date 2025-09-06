using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Counter.Entities;
using NetDream.Modules.Counter.Migrations;

namespace NetDream.Modules.Counter
{
    public class CounterContext(DbContextOptions<CounterContext> options): DbContext(options)
    {
        public DbSet<ClickLogEntity> ClickLogs {get; set; }
        public DbSet<JumpLogEntity> JumpLogs {get; set; }
        public DbSet<LoadTimeLogEntity> LoadTimeLogs {get; set; }
        public DbSet<LogEntity> Logs {get; set; }
        public DbSet<PageLogEntity> PageLogs {get; set; }
        public DbSet<StayTimeLogEntity> StayTimeLogs {get; set; }
        public DbSet<VisitorLogEntity> VisitorLogs {get; set; }
        public DbSet<HostnameEntity> Hostnames {get; set; }
        public DbSet<PathnameEntity> Pathnames { get; set; }
        public DbSet<SiteEntity> Sites { get; set; }
        public DbSet<AnalysisFlagEntity> AnalysisFlags { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClickLogEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new JumpLogEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new LoadTimeLogEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new LogEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PageLogEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new StayTimeLogEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new VisitorLogEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new HostnameEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PathnameEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SiteEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new AnalysisFlagEntityTypeConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
