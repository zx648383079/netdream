using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Auth.Entities;
using NetDream.Modules.Auth.Migrations;
using NetDream.Shared.Providers.Context;
using NetDream.Shared.Providers.Entities;
using NetDream.Shared.Providers.Migrations;

namespace NetDream.Modules.Auth
{
    public class AuthContext(DbContextOptions<AuthContext> options): 
        DbContext(options)
    {

        public DbSet<BanAccountEntity> BanAccounts {get; set; }
        public DbSet<InviteCodeEntity> InviteCodes {get; set; }
        public DbSet<InviteLogEntity> InviteLogs {get; set; }
        public DbSet<LoginLogEntity> LoginLogs {get; set; }
        public DbSet<LoginQrEntity> LoginQrs {get; set; }
    
        public DbSet<OauthEntity> OAuth {get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
          
            modelBuilder.ApplyConfiguration(new BanAccountEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new InviteCodeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new InviteLogEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new LoginLogEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new LoginQrEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OAuthEntityTypeConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
