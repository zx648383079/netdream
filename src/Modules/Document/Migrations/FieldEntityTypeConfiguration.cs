using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Document.Entities;

namespace NetDream.Modules.Document.Migrations
{
    public class FieldEntityTypeConfiguration : IEntityTypeConfiguration<FieldEntity>
    {
        public void Configure(EntityTypeBuilder<FieldEntity> builder)
        {
            builder.ToTable("doc_field", table => table.HasComment("��Ŀ�ֶα�"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(50).HasComment("�ӿ�����");
            builder.Property(table => table.Title).HasColumnName("title").HasMaxLength(50).HasDefaultValue(string.Empty).HasComment("�ӿڱ���");
            builder.Property(table => table.IsRequired).HasColumnName("is_required").HasDefaultValue(1).HasComment("�Ƿ�ش�");
            builder.Property(table => table.DefaultValue).HasColumnName("default_value").HasDefaultValue(string.Empty).HasComment("Ĭ��ֵ");
            builder.Property(table => table.Mock).HasColumnName("mock").HasDefaultValue(string.Empty).HasComment("mock����");
            builder.Property(table => table.ParentId).HasColumnName("parent_id").HasDefaultValue(0);
            builder.Property(table => table.ApiId).HasColumnName("api_id").HasComment("�ӿ�id");
            builder.Property(table => table.Kind).HasColumnName("kind").HasMaxLength(2).HasDefaultValue(1).HasComment("�������ͣ�1:�����ֶ� 2:��Ӧ�ֶ� 3:header�ֶ�");
            builder.Property(table => table.Type).HasColumnName("type").HasMaxLength(10).HasDefaultValue(string.Empty).HasComment("�ֶ�����");
            builder.Property(table => table.Remark).HasColumnName("remark").HasComment("��ע");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
