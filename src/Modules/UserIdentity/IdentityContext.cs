using Microsoft.EntityFrameworkCore;
using NetDream.Modules.UserIdentity.Entities;
using NetDream.Modules.UserIdentity.Migrations;

namespace NetDream.Modules.UserIdentity
{
    public class IdentityContext(DbContextOptions<IdentityContext> options) : DbContext(options)
    {
        public DbSet<EquityCardEntity> EquityCards { get; set; }
        public DbSet<UserEquityCardEntity> UserEquityCards { get; set; }
        public DbSet<RankEntity> Ranks { get; set; }
        public DbSet<PermissionEntity> Permissions { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<RolePermissionEntity> RolePermissions { get; set; }
        public DbSet<UserRoleEntity> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
