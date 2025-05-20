using Microsoft.EntityFrameworkCore;
using NetDream.Modules.OnlineMedia.Entities;
using NetDream.Modules.OnlineMedia.Migrations;

namespace NetDream.Modules.OnlineMedia;
public class OnlineMediaContext(DbContextOptions<OnlineMediaContext> options) : DbContext(options)
{
    public DbSet<AreaEntity> Area { get; set; }
    public DbSet<CategoryEntity> Category { get; set; }
    public DbSet<LiveEntity> Live { get; set; }
    public DbSet<LogEntity> Log { get; set; }
    public DbSet<MovieEntity> Movie { get; set; }
    public DbSet<MovieFileEntity> MovieFile { get; set; }
    public DbSet<MovieScoreEntity> MovieScore { get; set; }
    public DbSet<MovieSeriesEntity> MovieSeries { get; set; }
    public DbSet<MusicEntity> Music { get; set; }
    public DbSet<MusicFileEntity> MusicFile { get; set; }
    public DbSet<MusicListEntity> MusicList { get; set; }
    public DbSet<MusicListItemEntity> MusicListItem { get; set; }
    public DbSet<TagEntity> Tag { get; set; }
    public DbSet<TagLinkEntity> TagLink { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AreaEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new LiveEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new LogEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new MovieEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new MovieFileEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new MovieScoreEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new MovieSeriesEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new MusicEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new MusicFileEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new MusicListEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new MusicListItemEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new TagEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new TagLinkEntityTypeConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}