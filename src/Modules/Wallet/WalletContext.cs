using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Wallet.Entities;
using NetDream.Modules.Wallet.Migrations;

namespace NetDream.Modules.Wallet
{
    public class WalletContext(DbContextOptions<WalletContext> options): DbContext(options)
    {
        public DbSet<WalletEntity> Wallets {get; set; }
        public DbSet<WalletLogEntity> Logs {get; set; }
        public DbSet<WalletAccountEntity> Acounts {get; set; }

        public DbSet<AccountLogEntity> AccountLogs { get; set; }
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
