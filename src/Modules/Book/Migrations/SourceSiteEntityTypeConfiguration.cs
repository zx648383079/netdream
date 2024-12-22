using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Book.Entities;

namespace NetDream.Modules.Book.Migrations
{
    public class SourceSiteEntityTypeConfiguration : IEntityTypeConfiguration<SourceSiteEntity>
    {
        public void Configure(EntityTypeBuilder<SourceSiteEntity> builder)
        {
            builder.ToTable("book_source_site", table => table.HasComment("С˵��Դվ��"));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(30).HasComment("վ����");
            builder.Property(table => table.Url).HasColumnName("url").HasMaxLength(100).HasComment("��ַ");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}