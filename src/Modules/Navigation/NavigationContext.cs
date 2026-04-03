using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Navigation.Entities;
using NetDream.Modules.Navigation.Migrations;

namespace NetDream.Modules.Navigation
{
    public class NavigationContext(DbContextOptions<NavigationContext> options): DbContext(options)
    {
        public DbSet<CategoryEntity> Categories {get; set; }
        public DbSet<CollectEntity> Collects {get; set; }
        public DbSet<CollectGroupEntity> CollectGroups {get; set; }
        public DbSet<KeywordEntity> Keywords {get; set; }
        public DbSet<PageEntity> Pages {get; set; }
        public DbSet<PageKeywordEntity> PageKeywords {get; set; }
        public DbSet<SiteEntity> Sites {get; set; }
        public DbSet<SiteScoringLogEntity> SiteScoringLogs {get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CategoryEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CollectEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CollectGroupEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new KeywordEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PageEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PageKeywordEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SiteEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SiteScoringLogEntityTypeConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
