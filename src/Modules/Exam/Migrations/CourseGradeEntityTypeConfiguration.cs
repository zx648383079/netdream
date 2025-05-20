using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Exam.Entities;

namespace NetDream.Modules.Exam.Migrations;
public class CourseGradeEntityTypeConfiguration : IEntityTypeConfiguration<CourseGradeEntity>
{
    public void Configure(EntityTypeBuilder<CourseGradeEntity> builder)
    {
        builder.ToTable("exam_course_grade", table => table.HasComment("科目等级别名表"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.CourseId).HasColumnName("course_id").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(30);
        builder.Property(table => table.Grade).HasColumnName("grade").HasMaxLength(5).HasDefaultValue(1);
        
    }
}