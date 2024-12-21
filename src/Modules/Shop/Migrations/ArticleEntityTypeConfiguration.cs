using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Migrations
{
    public class ArticleEntityTypeConfiguration : IEntityTypeConfiguration<ArticleEntity>
    {
        public void Configure(EntityTypeBuilder<ArticleEntity> builder)
        {
            builder.ToTable("Article", table => table.HasComment(""));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.CatId).HasColumnName("cat_id");
            builder.Property(table => table.Title).HasColumnName("title").HasMaxLength(100).HasComment("������");
            builder.Property(table => table.Keywords).HasColumnName("keywords").HasMaxLength(200).HasDefaultValue(string.Empty).HasComment("�ؼ���");
            builder.Property(table => table.Thumb).HasColumnName("thumb").HasMaxLength(200).HasDefaultValue(string.Empty).HasComment("����ͼ");
            builder.Property(table => table.Description).HasColumnName("description").HasMaxLength(200).HasDefaultValue(string.Empty).HasComment("�ؼ���");
            builder.Property(table => table.Brief).HasColumnName("brief").HasMaxLength(200).HasDefaultValue(string.Empty).HasComment("���");
            builder.Property(table => table.Url).HasColumnName("url").HasMaxLength(200).HasDefaultValue(string.Empty).HasComment("����");
            builder.Property(table => table.File).HasColumnName("file").HasMaxLength(200).HasDefaultValue(string.Empty).HasComment("��������");
            builder.Property(table => table.Content).HasColumnName("content").HasComment("����");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
