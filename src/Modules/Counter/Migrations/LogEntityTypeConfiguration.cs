using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Counter.Entities;

namespace NetDream.Modules.Counter.Migrations
{
    public class LogEntityTypeConfiguration : IEntityTypeConfiguration<LogEntity>
    {
        public void Configure(EntityTypeBuilder<LogEntity> builder)
        {
            builder.ToTable("ctr_log", table => table.HasComment("访问记录"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Ip).HasColumnName("ip").HasMaxLength(120);
            builder.Property(table => table.Hostname).HasColumnName("hostname").HasMaxLength(100).HasDefaultValue(string.Empty).HasComment("域名");
            builder.Property(table => table.Pathname).HasColumnName("pathname").HasMaxLength(255).HasDefaultValue(string.Empty).HasComment("访问路径");
            builder.Property(table => table.Queries).HasColumnName("queries").HasMaxLength(255).HasDefaultValue(string.Empty).HasComment("查询参数");
            builder.Property(table => table.ReferrerHostname).HasColumnName("referrer_hostname").HasDefaultValue(string.Empty).HasComment("来路域名");
            builder.Property(table => table.ReferrerPathname).HasColumnName("referrer_pathname").HasDefaultValue(string.Empty).HasComment("来路路径及参数");
            builder.Property(table => table.Method).HasColumnName("method").HasMaxLength(10).HasDefaultValue("GET").HasComment("请求方法");
            builder.Property(table => table.StatusCode).HasColumnName("status_code").HasDefaultValue(200).HasComment("响应的状态码");
            builder.Property(table => table.UserAgent).HasColumnName("user_agent").HasDefaultValue(string.Empty).HasMaxLength(800).HasComment("代理");
            builder.Property(table => table.UserId).HasColumnName("user_id").HasDefaultValue(0);
            builder.Property(table => table.SessionId).HasColumnName("session_id").HasMaxLength(32).HasDefaultValue(string.Empty);
            builder.Property(table => table.Language).HasColumnName("language").HasMaxLength(20).HasDefaultValue(string.Empty);
            builder.Property(table => table.Latitude).HasColumnName("latitude").HasMaxLength(30).HasDefaultValue(string.Empty).HasComment("纬度");
            builder.Property(table => table.Longitude).HasColumnName("longitude").HasMaxLength(30).HasDefaultValue(string.Empty).HasComment("经度");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
