using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Counter.Entities;

namespace NetDream.Modules.Counter.Migrations
{
    public class ClickLogEntityTypeConfiguration : IEntityTypeConfiguration<ClickLogEntity>
    {
        public void Configure(EntityTypeBuilder<ClickLogEntity> builder)
        {
            builder.ToTable("ctr_click_log", table => table.HasComment("页面点击记录"));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Url).HasColumnName("url");
            builder.Property(table => table.Ip).HasColumnName("ip").HasMaxLength(120);
            builder.Property(table => table.SessionId).HasColumnName("session_id").HasMaxLength(32).HasDefaultValue(string.Empty);
            builder.Property(table => table.UserAgent).HasColumnName("user_agent").HasDefaultValue(string.Empty).HasComment("代理");
            builder.Property(table => table.X).HasColumnName("x").HasMaxLength(100).HasDefaultValue(0);
            builder.Property(table => table.Y).HasColumnName("y").HasMaxLength(100).HasDefaultValue(0);
            builder.Property(table => table.Tag).HasColumnName("tag").HasMaxLength(120);
            builder.Property(table => table.TagUrl).HasColumnName("tag_url").HasMaxLength(120).HasDefaultValue(string.Empty);
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
