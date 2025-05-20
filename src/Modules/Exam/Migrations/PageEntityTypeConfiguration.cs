using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Exam.Entities;

namespace NetDream.Modules.Exam.Migrations;
public class PageEntityTypeConfiguration : IEntityTypeConfiguration<PageEntity>
{
    public void Configure(EntityTypeBuilder<PageEntity> builder)
    {
        builder.ToTable("exam_page", table => table.HasComment("试卷集"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(200).HasComment("试卷名");
        builder.Property(table => table.RuleType).HasColumnName("rule_type").HasDefaultValue(0).HasComment("试卷生存类型");
        builder.Property(table => table.RuleValue).HasColumnName("rule_value").HasComment("试卷组成规则");
        builder.Property(table => table.StartAt).HasColumnName("start_at").HasMaxLength(10).HasDefaultValue(0).HasComment("开始时间");
        builder.Property(table => table.EndAt).HasColumnName("end_at").HasMaxLength(10).HasDefaultValue(0).HasComment("结束时间");
        builder.Property(table => table.LimitTime).HasColumnName("limit_time").HasMaxLength(5).HasDefaultValue(0).HasComment("限时");
        builder.Property(table => table.UserId).HasColumnName("user_id").HasMaxLength(10);
        builder.Property(table => table.Status).HasColumnName("status").HasDefaultValue(0);
        builder.Property(table => table.UpdatedAt).HasColumnName("updated_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.Score).HasColumnName("score").HasMaxLength(5).HasDefaultValue(0).HasComment("总分数");
        builder.Property(table => table.QuestionCount).HasColumnName("question_count").HasMaxLength(5).HasDefaultValue(0).HasComment("题目数");
        builder.Property(table => table.CourseId).HasColumnName("course_id").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.CourseGrade).HasColumnName("course_grade").HasMaxLength(5).HasDefaultValue(1);
        
    }
}