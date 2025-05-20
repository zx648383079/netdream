using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Career.Entities;

namespace NetDream.Modules.Career.Migrations;
public class InterviewLogEntityTypeConfiguration : IEntityTypeConfiguration<InterviewLogEntity>
{
    public void Configure(EntityTypeBuilder<InterviewLogEntity> builder)
    {
        builder.ToTable("career_interview_log", table => table.HasComment("面试记录"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.InterviewId).HasColumnName("interview_id").HasMaxLength(10);
        builder.Property(table => table.UserId).HasColumnName("user_id").HasMaxLength(10);
        builder.Property(table => table.Type).HasColumnName("type").HasDefaultValue(0);
        builder.Property(table => table.Data).HasColumnName("data").HasMaxLength(255).HasComment("附加数据");
        builder.Property(table => table.Status).HasColumnName("status").HasDefaultValue(0).HasComment("状态");
        builder.Property(table => table.UpdatedAt).HasColumnName("updated_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasMaxLength(10).HasDefaultValue(0);
        
    }
}