using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.OnlineMedia.Entities;

namespace NetDream.Modules.OnlineMedia.Migrations;
public class MusicEntityTypeConfiguration : IEntityTypeConfiguration<MusicEntity>
{
    public void Configure(EntityTypeBuilder<MusicEntity> builder)
    {
        builder.ToTable("tv_music", table => table.HasComment("音乐"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(255).HasComment("歌曲名");
        builder.Property(table => table.Cover).HasColumnName("cover").HasMaxLength(255).HasComment("封面");
        builder.Property(table => table.Album).HasColumnName("album").HasMaxLength(20).HasComment("专辑");
        builder.Property(table => table.Artist).HasColumnName("artist").HasMaxLength(20).HasComment("演唱者");
        builder.Property(table => table.Duration).HasColumnName("duration").HasMaxLength(10).HasDefaultValue(0).HasComment("歌曲长度");
        builder.Property(table => table.Status).HasColumnName("status").HasDefaultValue(0);
        builder.Property(table => table.UpdatedAt).HasColumnName("updated_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasMaxLength(10).HasDefaultValue(0);
        
    }
}