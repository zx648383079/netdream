using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Plan.Entities;

namespace NetDream.Modules.Plan.Migrations;
public class DayEntityTypeConfiguration : IEntityTypeConfiguration<DayEntity>
{
    public void Configure(EntityTypeBuilder<DayEntity> builder)
    {
        builder.ToTable("task_day", table => table.HasComment("每日代办任务"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.UserId).HasColumnName("user_id").HasMaxLength(10);
        builder.Property(table => table.TaskId).HasColumnName("task_id").HasMaxLength(10);
        builder.Property(table => table.Today).HasColumnName("today");
        builder.Property(table => table.Amount).HasColumnName("amount").HasDefaultValue(1).HasComment("剩余次数");
        builder.Property(table => table.SuccessAmount).HasColumnName("success_amount").HasDefaultValue(0).HasComment("成功次数");
        builder.Property(table => table.PauseAmount).HasColumnName("pause_amount").HasDefaultValue(0).HasComment("暂停次数");
        builder.Property(table => table.FailureAmount).HasColumnName("failure_amount").HasDefaultValue(0).HasComment("中断次数");
        builder.Property(table => table.Status).HasColumnName("status").HasDefaultValue(5);
        builder.Property(table => table.UpdatedAt).HasColumnName("updated_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasMaxLength(10).HasDefaultValue(0);
        
    }
}