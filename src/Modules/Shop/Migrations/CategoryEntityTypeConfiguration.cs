using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Migrations
{
    public class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<CategoryEntity>
    {
        public void Configure(EntityTypeBuilder<CategoryEntity> builder)
        {
            builder.ToTable("Category", table => table.HasComment(""));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(100).HasComment("分类名");
            builder.Property(table => table.Keywords).HasColumnName("keywords").HasMaxLength(200).HasComment("关键字");
            builder.Property(table => table.Description).HasColumnName("description").HasMaxLength(200).HasComment("关键字");
            builder.Property(table => table.Icon).HasColumnName("icon").HasMaxLength(200);
            builder.Property(table => table.Banner).HasColumnName("banner").HasMaxLength(200);
            builder.Property(table => table.AppBanner).HasColumnName("app_banner").HasMaxLength(200);
            builder.Property(table => table.ParentId).HasColumnName("parent_id").HasDefaultValue(0);
            builder.Property(table => table.Position).HasColumnName("position").HasMaxLength(3).HasDefaultValue(99);
        }
    }
}
