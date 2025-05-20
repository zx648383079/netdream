using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.OnlineMedia.Entities;

namespace NetDream.Modules.OnlineMedia.Migrations;
public class LiveEntityTypeConfiguration : IEntityTypeConfiguration<LiveEntity>
{
    public void Configure(EntityTypeBuilder<LiveEntity> builder)
    {
        builder.ToTable("tv_live", table => table.HasComment("直播源"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.Title).HasColumnName("title").HasMaxLength(255);
        builder.Property(table => table.Thumb).HasColumnName("thumb").HasMaxLength(255);
        builder.Property(table => table.Source).HasColumnName("source").HasMaxLength(255);
        builder.Property(table => table.Status).HasColumnName("status").HasDefaultValue(1);
        builder.Property(table => table.UpdatedAt).HasColumnName("updated_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasMaxLength(10).HasDefaultValue(0);
        
    }
}