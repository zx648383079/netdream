using Microsoft.EntityFrameworkCore;
using NetDream.Shared.Providers.Entities;
using NetDream.Shared.Providers.Migrations;

namespace NetDream.Shared.Providers.Context
{
    public class SketchContext(DbContextOptions<SketchContext> options) : DbContext(options)
    {

        public DbSet<SketchLogEntity> SketchLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new SketchLogEntityMigration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
