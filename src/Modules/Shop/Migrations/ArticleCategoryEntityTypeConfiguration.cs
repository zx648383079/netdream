using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Migrations
{
    public class ArticleCategoryEntityTypeConfiguration : IEntityTypeConfiguration<ArticleCategoryEntity>
    {
        public void Configure(EntityTypeBuilder<ArticleCategoryEntity> builder)
        {
            builder.ToTable("ArticleCategory", table => table.HasComment(""));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(100).HasComment("文章分类名");
            builder.Property(table => table.Keywords).HasColumnName("keywords").HasMaxLength(200).HasDefaultValue(string.Empty).HasComment("关键字");
            builder.Property(table => table.Description).HasColumnName("description").HasMaxLength(200).HasDefaultValue(string.Empty).HasComment("关键字");
            builder.Property(table => table.ParentId).HasColumnName("parent_id").HasDefaultValue(0);
            builder.Property(table => table.Position).HasColumnName("position").HasMaxLength(3).HasDefaultValue(99);
        }
    }
}
