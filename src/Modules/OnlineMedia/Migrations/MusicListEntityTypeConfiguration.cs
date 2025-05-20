using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.OnlineMedia.Entities;

namespace NetDream.Modules.OnlineMedia.Migrations;
public class MusicListEntityTypeConfiguration : IEntityTypeConfiguration<MusicListEntity>
{
    public void Configure(EntityTypeBuilder<MusicListEntity> builder)
    {
        builder.ToTable("tv_music_list", table => table.HasComment("音乐歌单"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.Title).HasColumnName("title").HasMaxLength(255).HasComment("歌单名");
        builder.Property(table => table.UserId).HasColumnName("user_id").HasMaxLength(10);
        builder.Property(table => table.Cover).HasColumnName("cover").HasMaxLength(255).HasComment("封面");
        builder.Property(table => table.Description).HasColumnName("description").HasMaxLength(255).HasComment("介绍");
        builder.Property(table => table.Status).HasColumnName("status").HasDefaultValue(0);
        builder.Property(table => table.UpdatedAt).HasColumnName("updated_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasMaxLength(10).HasDefaultValue(0);
        
    }
}