using Microsoft.EntityFrameworkCore;
using NetDream.Modules.UserAccount.Entities;
using NetDream.Modules.UserAccount.Migrations;
using NetDream.Shared.Providers.Context;
using NetDream.Shared.Providers.Entities;
using NetDream.Shared.Providers.Migrations;

namespace NetDream.Modules.UserAccount
{
    public class UserContext(DbContextOptions<UserContext> options) :
        DbContext(options), IMetaContext
    {
        public DbSet<AccountLogEntity> AccountLogs { get; set; }
        public DbSet<ActionLogEntity> ActionLogs { get; set; }
        public DbSet<AdminLogEntity> AdminLogs { get; set; }
        public DbSet<ApplyLogEntity> ApplyLogs { get; set; }

        public DbSet<CreditLogEntity> CreditLogs { get; set; }

        public DbSet<RelationshipEntity> Relationships { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<BulletinEntity> Bulletins { get; set; }
        public DbSet<BulletinUserEntity> BulletinUsers { get; set; }
        public DbSet<MetaEntity> Metas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AccountLogEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ActionLogEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new AdminLogEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ApplyLogEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CreditLogEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RelationshipEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new BulletinEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new BulletinUserEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new MetaEntityTypeConfiguration("user"));
            base.OnModelCreating(modelBuilder);
        }
    }
}
