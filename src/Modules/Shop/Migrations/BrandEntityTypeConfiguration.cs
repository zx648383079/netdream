using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Migrations
{
    public class BrandEntityTypeConfiguration : IEntityTypeConfiguration<BrandEntity>
    {
        public void Configure(EntityTypeBuilder<BrandEntity> builder)
        {
            builder.ToTable("Brand", table => table.HasComment(""));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(100).HasComment("分类名");
            builder.Property(table => table.Keywords).HasColumnName("keywords").HasMaxLength(200).HasComment("关键字");
            builder.Property(table => table.Description).HasColumnName("description").HasMaxLength(200).HasComment("关键字");
            builder.Property(table => table.Logo).HasColumnName("logo").HasMaxLength(200).HasComment("LOGO");
            builder.Property(table => table.AppLogo).HasColumnName("app_logo").HasMaxLength(200).HasComment("LOGO");
            builder.Property(table => table.Url).HasColumnName("url").HasMaxLength(200).HasComment("官网");
        }
    }
}
