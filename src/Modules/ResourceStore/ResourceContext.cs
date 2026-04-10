using Microsoft.EntityFrameworkCore;
using NetDream.Modules.ResourceStore.Entities;
using NetDream.Modules.ResourceStore.Migrations;

namespace NetDream.Modules.ResourceStore;
public class ResourceContext(DbContextOptions<ResourceContext> options) 
    : DbContext(options)
{
    public DbSet<ResourceEntity> Resources { get; set; }
    public DbSet<ResourceFileEntity> ResourceFiles { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ResourceEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ResourceFileEntityTypeConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}