using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Plan.Entities;

namespace NetDream.Modules.Plan.Migrations;
public class TaskEntityTypeConfiguration : IEntityTypeConfiguration<TaskEntity>
{
    public void Configure(EntityTypeBuilder<TaskEntity> builder)
    {
        builder.ToTable("task", table => table.HasComment("任务系统"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.UserId).HasColumnName("user_id").HasMaxLength(10);
        builder.Property(table => table.ParentId).HasColumnName("parent_id").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(100);
        builder.Property(table => table.Description).HasColumnName("description").HasMaxLength(255);
        builder.Property(table => table.Status).HasColumnName("status").HasDefaultValue(5);
        builder.Property(table => table.EveryTime).HasColumnName("every_time").HasMaxLength(5).HasDefaultValue(0).HasComment("每次计划时间");
        builder.Property(table => table.SpaceTime).HasColumnName("space_time").HasDefaultValue(0).HasComment("每次休息时间");
        builder.Property(table => table.StartAt).HasColumnName("start_at").HasMaxLength(10).HasDefaultValue(0).HasComment("任务开始时间");
        builder.Property(table => table.PerTime).HasColumnName("per_time").HasDefaultValue(0).HasComment("每天连续次数");
        builder.Property(table => table.TimeLength).HasColumnName("time_length").HasMaxLength(10).HasDefaultValue(0).HasComment("总时间");
        builder.Property(table => table.UpdatedAt).HasColumnName("updated_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasMaxLength(10).HasDefaultValue(0);
        
    }
}