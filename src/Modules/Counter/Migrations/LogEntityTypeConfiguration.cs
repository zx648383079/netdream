using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Counter.Entities;

namespace NetDream.Modules.Counter.Migrations
{
    public class LogEntityTypeConfiguration : IEntityTypeConfiguration<LogEntity>
    {
        public void Configure(EntityTypeBuilder<LogEntity> builder)
        {
            builder.ToTable("ctr_log", table => table.HasComment("���ʼ�¼"));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Ip).HasColumnName("ip").HasMaxLength(120);
            builder.Property(table => table.Browser).HasColumnName("browser").HasMaxLength(40).HasDefaultValue(string.Empty).HasComment("�����");
            builder.Property(table => table.BrowserVersion).HasColumnName("browser_version").HasMaxLength(20).HasDefaultValue(string.Empty).HasComment("������汾");
            builder.Property(table => table.Os).HasColumnName("os").HasMaxLength(20).HasDefaultValue(string.Empty).HasComment("����ϵͳ");
            builder.Property(table => table.OsVersion).HasColumnName("os_version").HasMaxLength(20).HasDefaultValue(string.Empty).HasComment("����ϵͳ�汾");
            builder.Property(table => table.Url).HasColumnName("url").HasDefaultValue(string.Empty).HasComment("������ַ");
            builder.Property(table => table.Referrer).HasColumnName("referrer").HasDefaultValue(string.Empty).HasComment("��·");
            builder.Property(table => table.UserAgent).HasColumnName("user_agent").HasDefaultValue(string.Empty).HasComment("����");
            builder.Property(table => table.Country).HasColumnName("country").HasMaxLength(45).HasDefaultValue(string.Empty);
            builder.Property(table => table.Region).HasColumnName("region").HasMaxLength(45).HasDefaultValue(string.Empty);
            builder.Property(table => table.City).HasColumnName("city").HasMaxLength(45).HasDefaultValue(string.Empty);
            builder.Property(table => table.UserId).HasColumnName("user_id").HasDefaultValue(0);
            builder.Property(table => table.SessionId).HasColumnName("session_id").HasMaxLength(32).HasDefaultValue(string.Empty);
            builder.Property(table => table.Language).HasColumnName("language").HasMaxLength(20).HasDefaultValue(string.Empty);
            builder.Property(table => table.Latitude).HasColumnName("latitude").HasMaxLength(30).HasDefaultValue(string.Empty).HasComment("γ��");
            builder.Property(table => table.Longitude).HasColumnName("longitude").HasMaxLength(30).HasDefaultValue(string.Empty).HasComment("����");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
