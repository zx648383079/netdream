using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.OnlineMedia.Entities;

namespace NetDream.Modules.OnlineMedia.Migrations;
public class MovieEntityTypeConfiguration : IEntityTypeConfiguration<MovieEntity>
{
    public void Configure(EntityTypeBuilder<MovieEntity> builder)
    {
        builder.ToTable("tv_movie", table => table.HasComment("影视"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.Title).HasColumnName("title").HasMaxLength(255);
        builder.Property(table => table.FilmTitle).HasColumnName("film_title").HasMaxLength(255);
        builder.Property(table => table.TranslationTitle).HasColumnName("translation_title").HasMaxLength(255);
        builder.Property(table => table.Cover).HasColumnName("cover").HasMaxLength(255).HasComment("封面");
        builder.Property(table => table.Director).HasColumnName("director").HasMaxLength(255).HasComment("导演");
        builder.Property(table => table.Leader).HasColumnName("leader").HasMaxLength(500).HasComment("主演");
        builder.Property(table => table.Screenwriter).HasColumnName("screenwriter").HasMaxLength(255).HasComment("编剧");
        builder.Property(table => table.CatId).HasColumnName("cat_id").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.AreaId).HasColumnName("area_id").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.Age).HasColumnName("age").HasMaxLength(4).HasDefaultValue("2025");
        builder.Property(table => table.Language).HasColumnName("language").HasMaxLength(10);
        builder.Property(table => table.ReleaseDate).HasColumnName("release_date").HasMaxLength(255).HasComment("上映日期");
        builder.Property(table => table.Duration).HasColumnName("duration").HasMaxLength(10).HasDefaultValue(0).HasComment("时长");
        builder.Property(table => table.Description).HasColumnName("description").HasMaxLength(255);
        builder.Property(table => table.Content).HasColumnName("content");
        builder.Property(table => table.SeriesCount).HasColumnName("series_count").HasMaxLength(5).HasDefaultValue(1).HasComment("一集就是电影");
        builder.Property(table => table.Status).HasColumnName("status").HasDefaultValue(0);
        builder.Property(table => table.UpdatedAt).HasColumnName("updated_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasMaxLength(10).HasDefaultValue(0);
        
    }
}