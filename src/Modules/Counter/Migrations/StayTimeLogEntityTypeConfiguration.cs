using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Counter.Entities;

namespace NetDream.Modules.Counter.Migrations
{
    public class StayTimeLogEntityTypeConfiguration : IEntityTypeConfiguration<StayTimeLogEntity>
    {
        public void Configure(EntityTypeBuilder<StayTimeLogEntity> builder)
        {
            builder.ToTable("ctr_stay_time_log", table => table.HasComment("ҳ��ͣ��ʱ��"));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Url).HasColumnName("url");
            builder.Property(table => table.Ip).HasColumnName("ip").HasMaxLength(120);
            builder.Property(table => table.UserAgent).HasColumnName("user_agent").HasDefaultValue(string.Empty).HasComment("����");
            builder.Property(table => table.SessionId).HasColumnName("session_id").HasMaxLength(32).HasDefaultValue(string.Empty);
            builder.Property(table => table.Status).HasColumnName("status").HasDefaultValue(0)
                .HasComment("�Ƿ�ͣ��");
            builder.Property(table => table.EnterAt).HasColumnName("enter_at");
            builder.Property(table => table.LeaveAt).HasColumnName("leave_at");
        }
    }
}
