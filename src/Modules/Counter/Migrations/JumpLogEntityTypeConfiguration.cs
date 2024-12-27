using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Counter.Entities;

namespace NetDream.Modules.Counter.Migrations
{
    public class JumpLogEntityTypeConfiguration : IEntityTypeConfiguration<JumpLogEntity>
    {
        public void Configure(EntityTypeBuilder<JumpLogEntity> builder)
        {
            builder.ToTable("ctr_jump_log", table => table.HasComment("页面跳出记录"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Referrer).HasColumnName("referrer").HasDefaultValue(string.Empty);
            builder.Property(table => table.Url).HasColumnName("url");
            builder.Property(table => table.Ip).HasColumnName("ip").HasMaxLength(120);
            builder.Property(table => table.SessionId).HasColumnName("session_id").HasMaxLength(32).HasDefaultValue(string.Empty);
            builder.Property(table => table.UserAgent).HasColumnName("user_agent").HasDefaultValue(string.Empty).HasComment("代理");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
