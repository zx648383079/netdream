using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Auth.Entities;
using NetDream.Modules.Auth.Migrations;

namespace NetDream.Modules.Auth
{
    public class AuthContext(DbContextOptions<AuthContext> options): DbContext(options)
    {
        public DbSet<AccountLogEntity> AccountLogs {get; set; }
        public DbSet<ActionLogEntity> ActionLogs {get; set; }
        public DbSet<AdminLogEntity> AdminLogs {get; set; }
        public DbSet<ApplyLogEntity> ApplyLogs {get; set; }
        public DbSet<BanAccountEntity> BanAccounts {get; set; }
        public DbSet<CreditLogEntity> CreditLogs {get; set; }
        public DbSet<InviteCodeEntity> InviteCodes {get; set; }
        public DbSet<InviteLogEntity> InviteLogs {get; set; }
        public DbSet<LoginLogEntity> LoginLogs {get; set; }
        public DbSet<LoginQrEntity> LoginQrs {get; set; }
        public DbSet<MailLogEntity> MailLogs {get; set; }
        public DbSet<MetaEntity> Metas {get; set; }
        public DbSet<OauthEntity> Oauths {get; set; }
        public DbSet<RelationshipEntity> Relationships {get; set; }
        public DbSet<UserEntity> Users {get; set; }
        public DbSet<BulletinEntity> Bulletins {get; set; }
        public DbSet<BulletinUserEntity> BulletinUsers {get; set; }
        public DbSet<EquityCardEntity> EquityCards {get; set; }
        public DbSet<UserEquityCardEntity> UserEquityCards {get; set; }
        public DbSet<RankEntity> Ranks {get; set; }
        public DbSet<PermissionEntity> Permissions {get; set; }
        public DbSet<RoleEntity> Roles {get; set; }
        public DbSet<RolePermissionEntity> RolePermissions {get; set; }
        public DbSet<UserRoleEntity> UserRoles {get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AccountLogEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ActionLogEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new AdminLogEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ApplyLogEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new BanAccountEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CreditLogEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new InviteCodeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new InviteLogEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new LoginLogEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new LoginQrEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new MailLogEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new MetaEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OauthEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RelationshipEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new BulletinEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new BulletinUserEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new EquityCardEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserEquityCardEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RankEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PermissionEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RoleEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RolePermissionEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleEntityTypeConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
