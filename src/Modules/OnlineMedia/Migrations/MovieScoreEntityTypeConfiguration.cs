using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.OnlineMedia.Entities;

namespace NetDream.Modules.OnlineMedia.Migrations;
public class MovieScoreEntityTypeConfiguration : IEntityTypeConfiguration<MovieScoreEntity>
{
    public void Configure(EntityTypeBuilder<MovieScoreEntity> builder)
    {
        builder.ToTable("tv_movie_score", table => table.HasComment(""));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.MovieId).HasColumnName("movie_id").HasMaxLength(10);
        builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(20);
        builder.Property(table => table.Score).HasColumnName("score").HasMaxLength(10);
        builder.Property(table => table.Amount).HasColumnName("amount").HasMaxLength(10).HasDefaultValue(0).HasComment("参与评分人数");
        builder.Property(table => table.Url).HasColumnName("url").HasMaxLength(255).HasComment("评分站点页面");
        builder.Property(table => table.UpdatedAt).HasColumnName("updated_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasMaxLength(10).HasDefaultValue(0);
        
    }
}