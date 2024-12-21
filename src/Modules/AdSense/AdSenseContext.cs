using Microsoft.EntityFrameworkCore;
using NetDream.Modules.AdSense.Entities;
using NetDream.Modules.AdSense.Migrations;

namespace NetDream.Modules.AdSense
{
    public class AdSenseContext(DbContextOptions<AdSenseContext> options): DbContext(options)
    {
        public DbSet<AdEntity> Ads {get; set; }
        public DbSet<PositionEntity> Positions {get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AdEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PositionEntityTypeConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
