using Microsoft.EntityFrameworkCore;
using NetDream.Modules.TradeTracker.Entities;
using NetDream.Modules.TradeTracker.Migrations;

namespace NetDream.Modules.TradeTracker
{
    public class TrackerContext(DbContextOptions<TrackerContext> options) : DbContext(options)
    {

        public DbSet<ChannelProductEntity> ChannelProducts { get; set; }
        public DbSet<ChannelEntity> Channels { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<TradeLogEntity> Logs { get; set; }
        public DbSet<TradeEntity> Trades { get; set; }
        public DbSet<UserProductEntity> UserProducts { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ChannelProductEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ChannelEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProductEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new TradeLogEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new TradeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserProductEntityTypeConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
