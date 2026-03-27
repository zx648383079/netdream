using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Storage.Entities;
using NetDream.Modules.Storage.Migrations;

namespace NetDream.Modules.Storage
{
    public class StorageContext(DbContextOptions<StorageContext> options) : DbContext(options)
    {

        public DbSet<FileEntity> Files { get; set; }
        public DbSet<FileLogEntity> Logs { get; set; }
        public DbSet<FileQuoteEntity> Quotes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FileEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new FileLogEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new FileQuoteEntityTypeConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
