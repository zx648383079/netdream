using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.OnlineMedia.Entities;

namespace NetDream.Modules.OnlineMedia.Migrations;
public class TagLinkEntityTypeConfiguration : IEntityTypeConfiguration<TagLinkEntity>
{
    public void Configure(EntityTypeBuilder<TagLinkEntity> builder)
    {
        builder.ToTable("tv_tag_link", table => table.HasComment(""));
        builder.Property(table => table.TagId).HasColumnName("tag_id").HasMaxLength(10);
        builder.Property(table => table.TargetId).HasColumnName("target_id").HasMaxLength(10);
        
    }
}