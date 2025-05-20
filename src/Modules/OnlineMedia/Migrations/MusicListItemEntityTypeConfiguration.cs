using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.OnlineMedia.Entities;

namespace NetDream.Modules.OnlineMedia.Migrations;
public class MusicListItemEntityTypeConfiguration : IEntityTypeConfiguration<MusicListItemEntity>
{
    public void Configure(EntityTypeBuilder<MusicListItemEntity> builder)
    {
        builder.ToTable("tv_music_list_item", table => table.HasComment("音乐歌单关联数据"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.ListId).HasColumnName("list_id").HasMaxLength(10);
        builder.Property(table => table.MusicId).HasColumnName("music_id").HasMaxLength(10);
        
    }
}