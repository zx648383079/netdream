using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Trade.Entities;
using NetDream.Modules.Trade.Migrations;

namespace NetDream.Modules.Trade
{
    public class TradeContext(DbContextOptions<TradeContext> options): DbContext(options)
    {
        public DbSet<TradeEntity> Trades {get; set; }
        public DbSet<RefundEntity> Refunds { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TradeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RefundEntityTypeConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
