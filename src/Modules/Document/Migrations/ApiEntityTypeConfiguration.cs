using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Document.Entities;

namespace NetDream.Modules.Document.Migrations
{
    public class ApiEntityTypeConfiguration : IEntityTypeConfiguration<ApiEntity>
    {
        public void Configure(EntityTypeBuilder<ApiEntity> builder)
        {
            builder.ToTable("doc_api", table => table.HasComment("��Ŀ�ӿڱ�"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(35).HasComment("�ӿ���");
            builder.Property(table => table.Type).HasColumnName("type").HasDefaultValue(0).HasComment("�Ƿ�������,0Ϊ������");
            builder.Property(table => table.Method).HasColumnName("method").HasMaxLength(10).HasDefaultValue("POST").HasComment("����ʽ");
            builder.Property(table => table.Uri).HasColumnName("uri").HasDefaultValue(string.Empty).HasComment("�ӿڵ�ַ");
            builder.Property(table => table.ProjectId).HasColumnName("project_id").HasComment("��Ŀ");
            builder.Property(table => table.VersionId).HasColumnName("version_id").HasDefaultValue(0).HasComment("�汾");
            builder.Property(table => table.Description).HasColumnName("description").HasDefaultValue(string.Empty).HasComment("�ӿڼ��");
            builder.Property(table => table.ParentId).HasColumnName("parent_id").HasDefaultValue(0);
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
