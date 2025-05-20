using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Exam.Entities;

namespace NetDream.Modules.Exam.Migrations;
public class QuestionOptionEntityTypeConfiguration : IEntityTypeConfiguration<QuestionOptionEntity>
{
    public void Configure(EntityTypeBuilder<QuestionOptionEntity> builder)
    {
        builder.ToTable("exam_question_option", table => table.HasComment("题选择选项"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.Content).HasColumnName("content").HasMaxLength(255);
        builder.Property(table => table.QuestionId).HasColumnName("question_id").HasMaxLength(10);
        builder.Property(table => table.Type).HasColumnName("type").HasDefaultValue(0);
        builder.Property(table => table.IsRight).HasColumnName("is_right").HasDefaultValue(0).HasComment("是否是正确答案");
        
    }
}