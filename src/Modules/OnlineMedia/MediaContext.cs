using Microsoft.EntityFrameworkCore;
using NetDream.Modules.OnlineMedia.Entities;
using NetDream.Modules.OnlineMedia.Migrations;

namespace NetDream.Modules.OnlineMedia;
public class MediaContext(DbContextOptions<MediaContext> options) 
    : DbContext(options)
{
    public DbSet<AreaEntity> Areas { get; set; }
    public DbSet<CategoryEntity> Categories { get; set; }
    public DbSet<LiveEntity> Lives { get; set; }
    public DbSet<MovieEntity> Movies { get; set; }
    public DbSet<MovieFileEntity> MovieFiles { get; set; }
    public DbSet<MovieScoreEntity> MovieScores { get; set; }
    public DbSet<MovieSeriesEntity> MovieSeries { get; set; }
    public DbSet<MusicEntity> Music { get; set; }
    public DbSet<MusicFileEntity> MusicFiles { get; set; }
    public DbSet<MusicListEntity> MusicLists { get; set; }
    public DbSet<MusicListItemEntity> MusicListItems { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AreaEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new LiveEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new MovieEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new MovieFileEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new MovieScoreEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new MovieSeriesEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new MusicEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new MusicFileEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new MusicListEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new MusicListItemEntityTypeConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}