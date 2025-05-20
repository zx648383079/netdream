using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Exam.Entities;

namespace NetDream.Modules.Exam.Migrations;
public class QuestionMaterialEntityTypeConfiguration : IEntityTypeConfiguration<QuestionMaterialEntity>
{
    public void Configure(EntityTypeBuilder<QuestionMaterialEntity> builder)
    {
        builder.ToTable("exam_question_material", table => table.HasComment("题选素材库"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.CourseId).HasColumnName("course_id").HasMaxLength(10);
        builder.Property(table => table.Title).HasColumnName("title").HasMaxLength(255);
        builder.Property(table => table.Description).HasColumnName("description").HasMaxLength(255);
        builder.Property(table => table.Type).HasColumnName("type").HasDefaultValue(0);
        builder.Property(table => table.Content).HasColumnName("content");
        
    }
}