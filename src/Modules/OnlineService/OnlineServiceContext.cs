using Microsoft.EntityFrameworkCore;
using NetDream.Modules.OnlineService.Entities;
using NetDream.Modules.OnlineService.Migrations;

namespace NetDream.Modules.OnlineService
{
    public class OnlineServiceContext(DbContextOptions<OnlineServiceContext> options): DbContext(options)
    {
        public DbSet<CategoryEntity> Categorys {get; set; }
        public DbSet<CategoryUserEntity> CategoryUsers {get; set; }
        public DbSet<CategoryWordEntity> CategoryWords {get; set; }
        public DbSet<MessageEntity> Messages {get; set; }
        public DbSet<SessionEntity> Sessions {get; set; }
        public DbSet<SessionLogEntity> SessionLogs {get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CategoryEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryUserEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryWordEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new MessageEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SessionEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SessionLogEntityTypeConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
