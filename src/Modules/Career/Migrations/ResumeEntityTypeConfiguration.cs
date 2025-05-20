using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Career.Entities;

namespace NetDream.Modules.Career.Migrations;
public class ResumeEntityTypeConfiguration : IEntityTypeConfiguration<ResumeEntity>
{
    public void Configure(EntityTypeBuilder<ResumeEntity> builder)
    {
        builder.ToTable("career_resume", table => table.HasComment("求职期望/建立"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.UserId).HasColumnName("user_id").HasMaxLength(10);
        builder.Property(table => table.Salary).HasColumnName("salary").HasMaxLength(10).HasDefaultValue(0.00).HasComment("月薪");
        builder.Property(table => table.SalaryRule).HasColumnName("salary_rule").HasDefaultValue(0).HasComment("工资方式");
        builder.Property(table => table.JobType).HasColumnName("job_type").HasDefaultValue(0).HasComment("工作方式：全职/兼职/实习");
        builder.Property(table => table.OnAnytime).HasColumnName("on_anytime").HasDefaultValue(0).HasComment("是否随时到岗");
        builder.Property(table => table.WorkDate).HasColumnName("work_date").HasMaxLength(10).HasDefaultValue(0).HasComment("到岗日期");
        builder.Property(table => table.WeeklyDays).HasColumnName("weekly_days").HasDefaultValue(0).HasComment("一周工作天数");
        builder.Property(table => table.EmployPeriod).HasColumnName("employ_period").HasDefaultValue(0).HasComment("工作期限/月");
        builder.Property(table => table.UpdatedAt).HasColumnName("updated_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasMaxLength(10).HasDefaultValue(0);
        
    }
}