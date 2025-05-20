using Microsoft.EntityFrameworkCore;
using NetDream.Modules.VisualEditor.Entities;
using NetDream.Modules.VisualEditor.Migrations;

namespace NetDream.Modules.VisualEditor;
public class VisualEditorContext(DbContextOptions<VisualEditorContext> options) : DbContext(options)
{
    public DbSet<SiteEntity> Site { get; set; }
    public DbSet<SiteComponentEntity> SiteComponent { get; set; }
    public DbSet<SitePageEntity> SitePage { get; set; }
    public DbSet<SitePageWeightEntity> SitePageWeight { get; set; }
    public DbSet<SiteWeightEntity> SiteWeight { get; set; }
    public DbSet<ThemeCategoryEntity> ThemeCategory { get; set; }
    public DbSet<ThemeComponentEntity> ThemeComponent { get; set; }
    public DbSet<ThemeStyleEntity> ThemeStyle { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new SiteEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new SiteComponentEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new SitePageEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new SitePageWeightEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new SiteWeightEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ThemeCategoryEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ThemeComponentEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ThemeStyleEntityTypeConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}