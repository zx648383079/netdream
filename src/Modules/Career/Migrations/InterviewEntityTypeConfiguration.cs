using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Career.Entities;

namespace NetDream.Modules.Career.Migrations;
public class InterviewEntityTypeConfiguration : IEntityTypeConfiguration<InterviewEntity>
{
    public void Configure(EntityTypeBuilder<InterviewEntity> builder)
    {
        builder.ToTable("career_interview", table => table.HasComment("面试"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.JobId).HasColumnName("job_id").HasMaxLength(10);
        builder.Property(table => table.CompanyId).HasColumnName("company_id").HasMaxLength(10);
        builder.Property(table => table.HrId).HasColumnName("hr_id").HasMaxLength(10);
        builder.Property(table => table.UserId).HasColumnName("user_id").HasMaxLength(10);
        builder.Property(table => table.Address).HasColumnName("address").HasMaxLength(255);
        builder.Property(table => table.InterviewAt).HasColumnName("interview_at").HasMaxLength(10).HasDefaultValue(0).HasComment("下次面试时间");
        builder.Property(table => table.EndAt).HasColumnName("end_at").HasMaxLength(10).HasDefaultValue(0).HasComment("面试通知截止时间");
        builder.Property(table => table.Status).HasColumnName("status").HasDefaultValue(0).HasComment("状态");
        builder.Property(table => table.UpdatedAt).HasColumnName("updated_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasMaxLength(10).HasDefaultValue(0);
        
    }
}