using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Document.Entities;

namespace NetDream.Modules.Document.Migrations
{
    public class ProjectEntityTypeConfiguration : IEntityTypeConfiguration<ProjectEntity>
    {
        public void Configure(EntityTypeBuilder<ProjectEntity> builder)
        {
            builder.ToTable("doc_project", table => table.HasComment("项目表"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.CatId).HasColumnName("cat_id").HasDefaultValue(0);
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(35).HasComment("项目名");
            builder.Property(table => table.Cover).HasColumnName("cover").HasMaxLength(200).HasDefaultValue(string.Empty)
                .HasComment("项目封面");
            builder.Property(table => table.Type).HasColumnName("type").HasMaxLength(1).HasDefaultValue(0).HasComment("文档类型");
            builder.Property(table => table.Description).HasColumnName("description").HasDefaultValue(string.Empty).HasComment("描述");
            builder.Property(table => table.Environment).HasColumnName("environment").HasComment("环境域名,json字符串");
            builder.Property(table => table.Status).HasColumnName("status").HasMaxLength(2).HasDefaultValue(0).HasComment("是否可见");
            builder.Property(table => table.DeletedAt).HasColumnName("deleted_at");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
