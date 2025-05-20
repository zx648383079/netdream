using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Exam.Entities;

namespace NetDream.Modules.Exam.Migrations;
public class QuestionEntityTypeConfiguration : IEntityTypeConfiguration<QuestionEntity>
{
    public void Configure(EntityTypeBuilder<QuestionEntity> builder)
    {
        builder.ToTable("exam_question", table => table.HasComment("题库"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.Title).HasColumnName("title").HasMaxLength(255);
        builder.Property(table => table.Image).HasColumnName("image").HasMaxLength(200);
        builder.Property(table => table.CourseId).HasColumnName("course_id").HasMaxLength(10);
        builder.Property(table => table.MaterialId).HasColumnName("material_id").HasMaxLength(10).HasDefaultValue(0).HasComment("题目素材");
        builder.Property(table => table.ParentId).HasColumnName("parent_id").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.Type).HasColumnName("type").HasDefaultValue(0).HasComment("题目类型");
        builder.Property(table => table.Easiness).HasColumnName("easiness").HasDefaultValue(0).HasComment("难易程度");
        builder.Property(table => table.Content).HasColumnName("content").HasComment("题目内容");
        builder.Property(table => table.Dynamic).HasColumnName("dynamic").HasComment("动态内容");
        builder.Property(table => table.Answer).HasColumnName("answer").HasComment("题目答案");
        builder.Property(table => table.UserId).HasColumnName("user_id").HasMaxLength(10);
        builder.Property(table => table.Status).HasColumnName("status").HasDefaultValue(0);
        builder.Property(table => table.UpdatedAt).HasColumnName("updated_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.CourseGrade).HasColumnName("course_grade").HasMaxLength(5).HasDefaultValue(1);
        
    }
}