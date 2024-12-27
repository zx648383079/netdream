using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Document.Entities;

namespace NetDream.Modules.Document.Migrations
{
    public class ApiEntityTypeConfiguration : IEntityTypeConfiguration<ApiEntity>
    {
        public void Configure(EntityTypeBuilder<ApiEntity> builder)
        {
            builder.ToTable("doc_api", table => table.HasComment("项目接口表"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(35).HasComment("接口名");
            builder.Property(table => table.Type).HasColumnName("type").HasDefaultValue(0).HasComment("是否有内容,0为有内容");
            builder.Property(table => table.Method).HasColumnName("method").HasMaxLength(10).HasDefaultValue("POST").HasComment("请求方式");
            builder.Property(table => table.Uri).HasColumnName("uri").HasDefaultValue(string.Empty).HasComment("接口地址");
            builder.Property(table => table.ProjectId).HasColumnName("project_id").HasComment("项目");
            builder.Property(table => table.VersionId).HasColumnName("version_id").HasDefaultValue(0).HasComment("版本");
            builder.Property(table => table.Description).HasColumnName("description").HasDefaultValue(string.Empty).HasComment("接口简介");
            builder.Property(table => table.ParentId).HasColumnName("parent_id").HasDefaultValue(0);
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
