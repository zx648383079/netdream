using Microsoft.EntityFrameworkCore;
using NetDream.Modules.OnlineDisk.Entities;
using NetDream.Modules.OnlineDisk.Migrations;

namespace NetDream.Modules.OnlineDisk
{
    public class OnlineDiskContext(DbContextOptions<OnlineDiskContext> options): DbContext(options)
    {
        public DbSet<ClientFileEntity> ClientFiles {get; set; }
        public DbSet<DiskEntity> Disks {get; set; }
        public DbSet<FileEntity> Files {get; set; }
        public DbSet<ServerEntity> Servers {get; set; }
        public DbSet<ServerFileEntity> ServerFiles {get; set; }
        public DbSet<ShareEntity> Shares {get; set; }
        public DbSet<ShareFileEntity> ShareFiles {get; set; }
        public DbSet<ShareUserEntity> ShareUsers {get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClientFileEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new DiskEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new FileEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ServerEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ServerFileEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ShareEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ShareFileEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ShareUserEntityTypeConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
