using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Career.Entities;

namespace NetDream.Modules.Career.Migrations;
public class JobEntityTypeConfiguration : IEntityTypeConfiguration<JobEntity>
{
    public void Configure(EntityTypeBuilder<JobEntity> builder)
    {
        builder.ToTable("career_job", table => table.HasComment("工作职位"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(255);
        builder.Property(table => table.CompanyId).HasColumnName("company_id").HasMaxLength(10);
        builder.Property(table => table.UserId).HasColumnName("user_id").HasMaxLength(10);
        builder.Property(table => table.Type).HasColumnName("type").HasDefaultValue(0).HasComment("工作类型：全职/兼职/实习");
        builder.Property(table => table.Address).HasColumnName("address").HasMaxLength(255).HasComment("地址");
        builder.Property(table => table.RegionId).HasColumnName("region_id").HasMaxLength(10).HasDefaultValue(0).HasComment("城市");
        builder.Property(table => table.Description).HasColumnName("description").HasMaxLength(255).HasComment("介绍");
        builder.Property(table => table.MinSalary).HasColumnName("min_salary").HasMaxLength(10).HasDefaultValue(0.00).HasComment("日/月/年薪");
        builder.Property(table => table.MaxSalary).HasColumnName("max_salary").HasMaxLength(10).HasDefaultValue(0.00).HasComment("日/月/年薪");
        builder.Property(table => table.SalaryRule).HasColumnName("salary_rule").HasDefaultValue(0).HasComment("工资日/月/年薪");
        builder.Property(table => table.SalaryType).HasColumnName("salary_type").HasDefaultValue(0).HasComment("工资类别：12/13/14薪");
        builder.Property(table => table.Longitude).HasColumnName("longitude").HasMaxLength(50).HasComment("经度");
        builder.Property(table => table.Latitude).HasColumnName("latitude").HasMaxLength(50).HasComment("纬度");
        builder.Property(table => table.Content).HasColumnName("content").HasMaxLength(255);
        builder.Property(table => table.Education).HasColumnName("education").HasDefaultValue(0).HasComment("学历");
        builder.Property(table => table.WorkTime).HasColumnName("work_time").HasDefaultValue(0).HasComment("工作经验");
        builder.Property(table => table.Tags).HasColumnName("tags").HasMaxLength(255).HasComment("标签");
        builder.Property(table => table.TopType).HasColumnName("top_type").HasDefaultValue(0).HasComment("推荐类型/急招/限招/枪手");
        builder.Property(table => table.WeeklyDays).HasColumnName("weekly_days").HasDefaultValue(0).HasComment("一周工作天数");
        builder.Property(table => table.CheckPeriod).HasColumnName("check_period").HasDefaultValue(0).HasComment("试用期/月");
        builder.Property(table => table.EmployPeriod).HasColumnName("employ_period").HasDefaultValue(0).HasComment("工作期限/月");
        builder.Property(table => table.HeadCount).HasColumnName("head_count").HasDefaultValue(0).HasComment("限招数量");
        builder.Property(table => table.Status).HasColumnName("status").HasDefaultValue(0).HasComment("状态");
        builder.Property(table => table.UpdatedAt).HasColumnName("updated_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasMaxLength(10).HasDefaultValue(0);
        
    }
}