using Microsoft.EntityFrameworkCore;
using NetDream.Shared.Providers.Entities;
using NetDream.Shared.Providers.Migrations;

namespace NetDream.Shared.Providers.Context
{
    public class StorageContext(DbContextOptions<StorageContext> options) : DbContext(options)
    {

        public DbSet<FileEntity> Files { get; set; }
        public DbSet<FileLogEntity> FileLogs { get; set; }
        public DbSet<FileQuoteEntity> FileQuotes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FileEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new FileLogEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new FileQuoteEntityTypeConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
