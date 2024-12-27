using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Document.Entities;

namespace NetDream.Modules.Document.Migrations
{
    public class FieldEntityTypeConfiguration : IEntityTypeConfiguration<FieldEntity>
    {
        public void Configure(EntityTypeBuilder<FieldEntity> builder)
        {
            builder.ToTable("doc_field", table => table.HasComment("项目字段表"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(50).HasComment("接口名称");
            builder.Property(table => table.Title).HasColumnName("title").HasMaxLength(50).HasDefaultValue(string.Empty).HasComment("接口标题");
            builder.Property(table => table.IsRequired).HasColumnName("is_required").HasDefaultValue(1).HasComment("是否必传");
            builder.Property(table => table.DefaultValue).HasColumnName("default_value").HasDefaultValue(string.Empty).HasComment("默认值");
            builder.Property(table => table.Mock).HasColumnName("mock").HasDefaultValue(string.Empty).HasComment("mock规则");
            builder.Property(table => table.ParentId).HasColumnName("parent_id").HasDefaultValue(0);
            builder.Property(table => table.ApiId).HasColumnName("api_id").HasComment("接口id");
            builder.Property(table => table.Kind).HasColumnName("kind").HasMaxLength(2).HasDefaultValue(1).HasComment("参数类型，1:请求字段 2:响应字段 3:header字段");
            builder.Property(table => table.Type).HasColumnName("type").HasMaxLength(10).HasDefaultValue(string.Empty).HasComment("字段类型");
            builder.Property(table => table.Remark).HasColumnName("remark").HasComment("备注");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
