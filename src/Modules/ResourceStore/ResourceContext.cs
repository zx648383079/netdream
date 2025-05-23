using Microsoft.EntityFrameworkCore;
using NetDream.Modules.ResourceStore.Entities;
using NetDream.Modules.ResourceStore.Migrations;
using NetDream.Shared.Providers.Context;
using NetDream.Shared.Providers.Entities;
using NetDream.Shared.Providers.Migrations;

namespace NetDream.Modules.ResourceStore;
public class ResourceContext(DbContextOptions<ResourceContext> options) 
    : DbContext(options), ITagContext, ICommentContext, 
    ILogContext, ISoreContext, IMetaContext
{
    public DbSet<CategoryEntity> Categories { get; set; }
    public DbSet<CommentEntity> Comments { get; set; }
    public DbSet<LogEntity> Logs { get; set; }
    public DbSet<ResourceEntity> Resources { get; set; }
    public DbSet<ResourceFileEntity> ResourceFiles { get; set; }
    public DbSet<MetaEntity> Metas { get; set; }
    public DbSet<ScoreLogEntity> ScoreLogs { get; set; }
    public DbSet<TagEntity> Tags { get; set; }
    public DbSet<TagLinkEntity> TagLinks { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CategoryEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CommentEntityTypeConfiguration("res_"));
        modelBuilder.ApplyConfiguration(new LogEntityTypeConfiguration("res_"));
        modelBuilder.ApplyConfiguration(new ResourceEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ResourceFileEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new MetaEntityTypeConfiguration("res_resource_"));
        modelBuilder.ApplyConfiguration(new ScoreLogEntityTypeConfiguration("res_"));
        modelBuilder.ApplyConfiguration(new TagEntityTypeConfiguration("res_"));
        modelBuilder.ApplyConfiguration(new TagLinkEntityTypeConfiguration("res_"));
        base.OnModelCreating(modelBuilder);
    }
}