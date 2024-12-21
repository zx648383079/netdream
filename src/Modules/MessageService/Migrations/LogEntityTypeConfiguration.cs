using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.MessageService.Entities;
using NetDream.Modules.MessageService.Repositories;

namespace NetDream.Modules.MessageService.Migrations
{
    public class LogEntityTypeConfiguration : IEntityTypeConfiguration<LogEntity>
    {
        public void Configure(EntityTypeBuilder<LogEntity> builder)
        {
            builder.ToTable("ms_log", table => table.HasComment("���ŷ��ͼ�¼"));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.TemplateId).HasColumnName("template_id").HasDefaultValue(0).HasComment("ģ��id");
            builder.Property(table => table.TargetType).HasColumnName("target_type").HasMaxLength(1).HasComment("����������");
            builder.Property(table => table.Target).HasColumnName("target").HasMaxLength(100).HasComment("�����ߣ��ֻ���/����");
            builder.Property(table => table.TemplateName).HasColumnName("template_name").HasMaxLength(20).HasDefaultValue(string.Empty).HasComment("���ô���");
            builder.Property(table => table.Type).HasColumnName("type").HasMaxLength(1).HasDefaultValue(MessageProtocol.TYPE_TEXT)
                .HasComment("���ݵ�����");
            builder.Property(table => table.Title).HasColumnName("title").HasComment("���͵ı���");
            builder.Property(table => table.Content).HasColumnName("content").HasComment("���͵�����");
            builder.Property(table => table.Code).HasColumnName("code").HasMaxLength(50).HasDefaultValue(string.Empty).HasComment("���͵���֤��");
            builder.Property(table => table.Status).HasColumnName("status").HasMaxLength(1).HasDefaultValue(0).HasComment("����״̬");
            builder.Property(table => table.Message).HasColumnName("message").HasDefaultValue(string.Empty).HasComment("���ͽ�����ɹ�Ϊ��Ϣid,����Ϊ������Ϣ");
            builder.Property(table => table.Ip).HasColumnName("ip").HasMaxLength(120).HasDefaultValue(string.Empty).HasComment("������ip");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
