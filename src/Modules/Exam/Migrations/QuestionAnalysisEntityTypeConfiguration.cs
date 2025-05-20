using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Exam.Entities;

namespace NetDream.Modules.Exam.Migrations;
public class QuestionAnalysisEntityTypeConfiguration : IEntityTypeConfiguration<QuestionAnalysisEntity>
{
    public void Configure(EntityTypeBuilder<QuestionAnalysisEntity> builder)
    {
        builder.ToTable("exam_question_analysis", table => table.HasComment("题选解析"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.QuestionId).HasColumnName("question_id").HasMaxLength(10);
        builder.Property(table => table.Type).HasColumnName("type").HasDefaultValue(0);
        builder.Property(table => table.Content).HasColumnName("content");
        
    }
}