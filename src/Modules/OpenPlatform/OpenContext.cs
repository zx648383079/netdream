using Microsoft.EntityFrameworkCore;
using NetDream.Modules.OpenPlatform.Entities;
using NetDream.Modules.OpenPlatform.Migrations;

namespace NetDream.Modules.OpenPlatform
{
    public class OpenContext(DbContextOptions<OpenContext> options): DbContext(options)
    {
        public DbSet<PlatformEntity> Platforms {get; set; }
        public DbSet<PlatformOptionEntity> PlatformOptions {get; set; }
        public DbSet<UserTokenEntity> UserTokens {get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PlatformEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PlatformOptionEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserTokenEntityTypeConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
