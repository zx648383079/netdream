using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.OnlineMedia.Entities;

namespace NetDream.Modules.OnlineMedia.Migrations;
public class MusicFileEntityTypeConfiguration : IEntityTypeConfiguration<MusicFileEntity>
{
    public void Configure(EntityTypeBuilder<MusicFileEntity> builder)
    {
        builder.ToTable("tv_music_file", table => table.HasComment("音乐文件"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.MusicId).HasColumnName("music_id").HasMaxLength(10);
        builder.Property(table => table.FileType).HasColumnName("file_type").HasDefaultValue(0).HasComment("文件类型,不同音质,歌词");
        builder.Property(table => table.File).HasColumnName("file").HasMaxLength(255);
        builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasMaxLength(10).HasDefaultValue(0);
        
    }
}