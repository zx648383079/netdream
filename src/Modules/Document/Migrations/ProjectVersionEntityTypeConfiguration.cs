using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Document.Entities;

namespace NetDream.Modules.Document.Migrations
{
    public class ProjectVersionEntityTypeConfiguration : IEntityTypeConfiguration<ProjectVersionEntity>
    {
        public void Configure(EntityTypeBuilder<ProjectVersionEntity> builder)
        {
            builder.ToTable("doc_project_version", table => table.HasComment("��Ŀ�汾��"));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.ProjectId).HasColumnName("project_id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(20).HasComment("�汾��");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
