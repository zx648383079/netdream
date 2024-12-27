using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.OnlineService.Entities;

namespace NetDream.Modules.OnlineService.Migrations
{
    public class SessionEntityTypeConfiguration : IEntityTypeConfiguration<SessionEntity>
    {
        public void Configure(EntityTypeBuilder<SessionEntity> builder)
        {
            builder.ToTable("service_session", table => table.HasComment(""));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(20).HasDefaultValue(string.Empty);
            builder.Property(table => table.Remark).HasColumnName("remark").HasDefaultValue(string.Empty);
            builder.Property(table => table.UserId).HasColumnName("user_id").HasDefaultValue(0);
            builder.Property(table => table.ServiceId).HasColumnName("service_id").HasDefaultValue(0);
            builder.Property(table => table.Ip).HasColumnName("ip").HasMaxLength(120);
            builder.Property(table => table.UserAgent).HasColumnName("user_agent").HasMaxLength(255);
            builder.Property(table => table.Status).HasColumnName("status").HasMaxLength(1).HasDefaultValue(0);
            builder.Property(table => table.ServiceWord).HasColumnName("service_word").HasDefaultValue(0).HasComment("客服设置的自动回复");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
