using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Catering.Entities;

namespace NetDream.Modules.Catering.Migrations;
public class GoodsGalleryEntityTypeConfiguration : IEntityTypeConfiguration<GoodsGalleryEntity>
{
    public void Configure(EntityTypeBuilder<GoodsGalleryEntity> builder)
    {
        builder.ToTable("eat_goods_gallery", table => table.HasComment(""));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.GoodsId).HasColumnName("goods_id").HasMaxLength(10);
        builder.Property(table => table.Thumb).HasColumnName("thumb").HasMaxLength(255);
        builder.Property(table => table.FileType).HasColumnName("file_type").HasDefaultValue(0);
        builder.Property(table => table.File).HasColumnName("file").HasMaxLength(255);
        
    }
}