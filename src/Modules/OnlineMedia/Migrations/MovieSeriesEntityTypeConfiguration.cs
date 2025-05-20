using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.OnlineMedia.Entities;

namespace NetDream.Modules.OnlineMedia.Migrations;
public class MovieSeriesEntityTypeConfiguration : IEntityTypeConfiguration<MovieSeriesEntity>
{
    public void Configure(EntityTypeBuilder<MovieSeriesEntity> builder)
    {
        builder.ToTable("tv_movie_series", table => table.HasComment(""));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.MovieId).HasColumnName("movie_id").HasMaxLength(10);
        builder.Property(table => table.Episode).HasColumnName("episode").HasMaxLength(5).HasComment("第几集");
        builder.Property(table => table.Title).HasColumnName("title").HasMaxLength(255);
        builder.Property(table => table.UpdatedAt).HasColumnName("updated_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasMaxLength(10).HasDefaultValue(0);
        
    }
}