using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Document.Entities;

namespace NetDream.Modules.Document.Migrations
{
    public class PageEntityTypeConfiguration : IEntityTypeConfiguration<PageEntity>
    {
        public void Configure(EntityTypeBuilder<PageEntity> builder)
        {
            builder.ToTable("doc_page", table => table.HasComment("��Ŀ��ͨ�ĵ�ҳ"));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(35).HasComment("�ĵ�");
            builder.Property(table => table.ProjectId).HasColumnName("project_id").HasComment("��Ŀ");
            builder.Property(table => table.VersionId).HasColumnName("version_id").HasDefaultValue(0).HasComment("�汾");
            builder.Property(table => table.ParentId).HasColumnName("parent_id").HasDefaultValue(0);
            builder.Property(table => table.Type).HasColumnName("type").HasDefaultValue(0).HasComment("�Ƿ�������,0Ϊ������");
            builder.Property(table => table.Content).HasColumnName("content").HasComment("����");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
