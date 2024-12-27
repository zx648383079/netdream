using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.OnlineService.Entities;

namespace NetDream.Modules.OnlineService.Migrations
{
    public class SessionLogEntityTypeConfiguration : IEntityTypeConfiguration<SessionLogEntity>
    {
        public void Configure(EntityTypeBuilder<SessionLogEntity> builder)
        {
            builder.ToTable("service_session_log", table => table.HasComment(""));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.SessionId).HasColumnName("session_id");
            builder.Property(table => table.Remark).HasColumnName("remark").HasDefaultValue(string.Empty);
            builder.Property(table => table.Status).HasColumnName("status").HasMaxLength(1).HasDefaultValue(0);
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
