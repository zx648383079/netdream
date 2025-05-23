using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Plan.Entities;

namespace NetDream.Modules.Plan.Migrations;
public class PlanEntityTypeConfiguration : IEntityTypeConfiguration<PlanEntity>
{
    public void Configure(EntityTypeBuilder<PlanEntity> builder)
    {
        builder.ToTable("task_plan", table => table.HasComment("提前计划任务"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.UserId).HasColumnName("user_id").HasMaxLength(10);
        builder.Property(table => table.TaskId).HasColumnName("task_id").HasMaxLength(10);
        builder.Property(table => table.PlanType).HasColumnName("plan_type").HasDefaultValue(0).HasComment("计划类型：按天，按周，按月");
        builder.Property(table => table.PlanTime).HasColumnName("plan_time");
        builder.Property(table => table.Amount).HasColumnName("amount").HasDefaultValue(1).HasComment("剩余次数");
        builder.Property(table => table.Priority).HasColumnName("priority").HasDefaultValue(8).HasComment("优先级");
        builder.Property(table => table.UpdatedAt).HasColumnName("updated_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasMaxLength(10).HasDefaultValue(0);
        
    }
}