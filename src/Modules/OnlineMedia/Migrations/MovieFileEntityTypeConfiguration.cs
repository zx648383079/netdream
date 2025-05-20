using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.OnlineMedia.Entities;

namespace NetDream.Modules.OnlineMedia.Migrations;
public class MovieFileEntityTypeConfiguration : IEntityTypeConfiguration<MovieFileEntity>
{
    public void Configure(EntityTypeBuilder<MovieFileEntity> builder)
    {
        builder.ToTable("tv_movie_file", table => table.HasComment(""));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(255);
        builder.Property(table => table.MovieId).HasColumnName("movie_id").HasMaxLength(10);
        builder.Property(table => table.SeriesId).HasColumnName("series_id").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.FileType).HasColumnName("file_type").HasDefaultValue(0).HasComment("文件类型,文件还是种子");
        builder.Property(table => table.Definition).HasColumnName("definition").HasDefaultValue(0).HasComment("清晰度");
        builder.Property(table => table.File).HasColumnName("file").HasMaxLength(255);
        builder.Property(table => table.Size).HasColumnName("size").HasMaxLength(20).HasDefaultValue("0");
        
    }
}