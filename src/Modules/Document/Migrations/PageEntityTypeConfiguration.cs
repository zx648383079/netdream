using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Document.Entities;

namespace NetDream.Modules.Document.Migrations
{
    public class PageEntityTypeConfiguration : IEntityTypeConfiguration<PageEntity>
    {
        public void Configure(EntityTypeBuilder<PageEntity> builder)
        {
            builder.ToTable("doc_page", table => table.HasComment("项目普通文档页"));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(35).HasComment("文档");
            builder.Property(table => table.ProjectId).HasColumnName("project_id").HasComment("项目");
            builder.Property(table => table.VersionId).HasColumnName("version_id").HasDefaultValue(0).HasComment("版本");
            builder.Property(table => table.ParentId).HasColumnName("parent_id").HasDefaultValue(0);
            builder.Property(table => table.Type).HasColumnName("type").HasDefaultValue(0).HasComment("是否有内容,0为有内容");
            builder.Property(table => table.Content).HasColumnName("content").HasComment("内容");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
