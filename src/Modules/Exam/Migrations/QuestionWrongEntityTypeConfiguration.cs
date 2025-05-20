using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Exam.Entities;

namespace NetDream.Modules.Exam.Migrations;
public class QuestionWrongEntityTypeConfiguration : IEntityTypeConfiguration<QuestionWrongEntity>
{
    public void Configure(EntityTypeBuilder<QuestionWrongEntity> builder)
    {
        builder.ToTable("exam_question_wrong", table => table.HasComment("错题集"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.QuestionId).HasColumnName("question_id").HasMaxLength(10);
        builder.Property(table => table.UserId).HasColumnName("user_id").HasMaxLength(10);
        builder.Property(table => table.Frequency).HasColumnName("frequency").HasMaxLength(5).HasDefaultValue(1).HasComment("出错次数");
        builder.Property(table => table.UpdatedAt).HasColumnName("updated_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasMaxLength(10).HasDefaultValue(0);
        
    }
}