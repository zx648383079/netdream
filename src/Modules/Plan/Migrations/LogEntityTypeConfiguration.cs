using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Plan.Entities;

namespace NetDream.Modules.Plan.Migrations;
public class LogEntityTypeConfiguration : IEntityTypeConfiguration<LogEntity>
{
    public void Configure(EntityTypeBuilder<LogEntity> builder)
    {
        builder.ToTable("task_log", table => table.HasComment("任务记录系统"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.UserId).HasColumnName("user_id").HasMaxLength(10);
        builder.Property(table => table.TaskId).HasColumnName("task_id").HasMaxLength(10);
        builder.Property(table => table.ChildId).HasColumnName("child_id").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.DayId).HasColumnName("day_id").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.Status).HasColumnName("status").HasDefaultValue(0);
        builder.Property(table => table.OutageTime).HasColumnName("outage_time").HasMaxLength(5).HasDefaultValue(0).HasComment("打扰时间");
        builder.Property(table => table.EndAt).HasColumnName("end_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasMaxLength(10).HasDefaultValue(0);
        
    }
}