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
            builder.ToTable("ms_log", table => table.HasComment("短信发送记录"));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.TemplateId).HasColumnName("template_id").HasDefaultValue(0).HasComment("模板id");
            builder.Property(table => table.TargetType).HasColumnName("target_type").HasMaxLength(1).HasComment("接受者类型");
            builder.Property(table => table.Target).HasColumnName("target").HasMaxLength(100).HasComment("接受者，手机号/邮箱");
            builder.Property(table => table.TemplateName).HasColumnName("template_name").HasMaxLength(20).HasDefaultValue(string.Empty).HasComment("调用代码");
            builder.Property(table => table.Type).HasColumnName("type").HasMaxLength(1).HasDefaultValue(MessageProtocol.TYPE_TEXT)
                .HasComment("内容的类型");
            builder.Property(table => table.Title).HasColumnName("title").HasComment("发送的标题");
            builder.Property(table => table.Content).HasColumnName("content").HasComment("发送的内容");
            builder.Property(table => table.Code).HasColumnName("code").HasMaxLength(50).HasDefaultValue(string.Empty).HasComment("发送的验证码");
            builder.Property(table => table.Status).HasColumnName("status").HasMaxLength(1).HasDefaultValue(0).HasComment("发送状态");
            builder.Property(table => table.Message).HasColumnName("message").HasDefaultValue(string.Empty).HasComment("发送结果，成功为消息id,否则为错误信息");
            builder.Property(table => table.Ip).HasColumnName("ip").HasMaxLength(120).HasDefaultValue(string.Empty).HasComment("发送者ip");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
