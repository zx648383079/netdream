using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Exam.Entities;

namespace NetDream.Modules.Exam.Migrations;
public class PageQuestionEntityTypeConfiguration : IEntityTypeConfiguration<PageQuestionEntity>
{
    public void Configure(EntityTypeBuilder<PageQuestionEntity> builder)
    {
        builder.ToTable("exam_page_question", table => table.HasComment("每次试卷题目及用户回答，如果为固定则复制开始的"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.PageId).HasColumnName("page_id").HasMaxLength(10);
        builder.Property(table => table.EvaluateId).HasColumnName("evaluate_id").HasMaxLength(10);
        builder.Property(table => table.QuestionId).HasColumnName("question_id").HasMaxLength(10);
        builder.Property(table => table.UserId).HasColumnName("user_id").HasMaxLength(10);
        builder.Property(table => table.Content).HasColumnName("content").HasComment("题目内容");
        builder.Property(table => table.Answer).HasColumnName("answer").HasComment("用户回答");
        builder.Property(table => table.Status).HasColumnName("status").HasDefaultValue(0).HasComment("状态");
        builder.Property(table => table.UpdatedAt).HasColumnName("updated_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.MaxScore).HasColumnName("max_score").HasMaxLength(5).HasDefaultValue(0).HasComment("满分");
        builder.Property(table => table.Score).HasColumnName("score").HasMaxLength(5).HasDefaultValue(0).HasComment("获得的分数");
        builder.Property(table => table.AnswerType).HasColumnName("answer_type").HasDefaultValue(0).HasComment("回答类型/默认文字");
        builder.Property(table => table.Remark).HasColumnName("remark").HasMaxLength(255);
        
    }
}