using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Document.Entities;

namespace NetDream.Modules.Document.Migrations
{
    public class ProjectEntityTypeConfiguration : IEntityTypeConfiguration<ProjectEntity>
    {
        public void Configure(EntityTypeBuilder<ProjectEntity> builder)
        {
            builder.ToTable("doc_project", table => table.HasComment("��Ŀ��"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.CatId).HasColumnName("cat_id").HasDefaultValue(0);
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(35).HasComment("��Ŀ��");
            builder.Property(table => table.Cover).HasColumnName("cover").HasMaxLength(200).HasDefaultValue(string.Empty)
                .HasComment("��Ŀ����");
            builder.Property(table => table.Type).HasColumnName("type").HasMaxLength(1).HasDefaultValue(0).HasComment("�ĵ�����");
            builder.Property(table => table.Description).HasColumnName("description").HasDefaultValue(string.Empty).HasComment("����");
            builder.Property(table => table.Environment).HasColumnName("environment").HasComment("��������,json�ַ���");
            builder.Property(table => table.Status).HasColumnName("status").HasMaxLength(2).HasDefaultValue(0).HasComment("�Ƿ�ɼ�");
            builder.Property(table => table.DeletedAt).HasColumnName("deleted_at");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
