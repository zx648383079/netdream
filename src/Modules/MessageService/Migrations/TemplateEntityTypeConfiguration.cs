using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.MessageService.Entities;
using NetDream.Modules.MessageService.Repositories;

namespace NetDream.Modules.MessageService.Migrations
{
    public class TemplateEntityTypeConfiguration : IEntityTypeConfiguration<TemplateEntity>
    {
        public void Configure(EntityTypeBuilder<TemplateEntity> builder)
        {
            builder.ToTable("ms_template", table => table.HasComment("��Ϣģ��"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Title).HasColumnName("title").HasMaxLength(100).HasComment("����");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(20).HasComment("���ô���");
            builder.Property(table => table.Type).HasColumnName("type").HasMaxLength(1).HasDefaultValue(MessageProtocol.TYPE_TEXT)
                .HasComment("ģ�������");
            builder.Property(table => table.Data).HasColumnName("data").HasComment("ģ���ֶ�");
            builder.Property(table => table.Content).HasColumnName("content").HasComment("ģ������");
            builder.Property(table => table.TargetNo).HasColumnName("target_no").HasMaxLength(32).HasDefaultValue(string.Empty).HasComment("�ⲿ���");
            builder.Property(table => table.Status).HasColumnName("status").HasDefaultValue(0).HasComment("�Ƿ�����");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
