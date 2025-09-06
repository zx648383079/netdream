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
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.LogId).HasColumnName("log_id").HasComment("���ʵļ�¼");
            builder.Property(table => table.Status).HasColumnName("status").HasDefaultValue(0)
                .HasComment("�Ƿ�ͣ��");
            builder.Property(table => table.EnterAt).HasColumnName("enter_at");
            builder.Property(table => table.LeaveAt).HasColumnName("leave_at");
        }
    }
}
