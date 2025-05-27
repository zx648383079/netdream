using Microsoft.EntityFrameworkCore;
using NetDream.Modules.OnlineMedia.Entities;
using NetDream.Modules.OnlineMedia.Migrations;
using NetDream.Shared.Providers.Context;
using NetDream.Shared.Providers.Entities;
using NetDream.Shared.Providers.Migrations;

namespace NetDream.Modules.OnlineMedia;
public class MediaContext(DbContextOptions<MediaContext> options) 
    : DbContext(options), ILogContext, ITagContext
{
    public DbSet<AreaEntity> Areas { get; set; }
    public DbSet<CategoryEntity> Categories { get; set; }
    public DbSet<LiveEntity> Lives { get; set; }
    public DbSet<LogEntity> Logs { get; set; }
    public DbSet<MovieEntity> Movies { get; set; }
    public DbSet<MovieFileEntity> MovieFiles { get; set; }
    public DbSet<MovieScoreEntity> MovieScores { get; set; }
    public DbSet<MovieSeriesEntity> MovieSeries { get; set; }
    public DbSet<MusicEntity> Music { get; set; }
    public DbSet<MusicFileEntity> MusicFiles { get; set; }
    public DbSet<MusicListEntity> MusicLists { get; set; }
    public DbSet<MusicListItemEntity> MusicListItems { get; set; }
    public DbSet<TagEntity> Tags { get; set; }
    public DbSet<TagLinkEntity> TagLinks { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AreaEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new LiveEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new LogEntityTypeConfiguration("tv_"));
        modelBuilder.ApplyConfiguration(new MovieEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new MovieFileEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new MovieScoreEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new MovieSeriesEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new MusicEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new MusicFileEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new MusicListEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new MusicListItemEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new TagEntityTypeConfiguration("tv_"));
        modelBuilder.ApplyConfiguration(new TagLinkEntityTypeConfiguration("tv_"));
        base.OnModelCreating(modelBuilder);
    }
}