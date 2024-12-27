using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Migrations
{
    public class GoodsGalleryEntityTypeConfiguration : IEntityTypeConfiguration<GoodsGalleryEntity>
    {
        public void Configure(EntityTypeBuilder<GoodsGalleryEntity> builder)
        {
            builder.ToTable("GoodsGallery", table => table.HasComment(""));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.GoodsId).HasColumnName("goods_id");
            builder.Property(table => table.Type).HasColumnName("type").HasMaxLength(1).HasDefaultValue(0).HasComment("文件类型，图片或视频");
            builder.Property(table => table.Thumb).HasColumnName("thumb");
            builder.Property(table => table.File).HasColumnName("file");
        }
    }
}
