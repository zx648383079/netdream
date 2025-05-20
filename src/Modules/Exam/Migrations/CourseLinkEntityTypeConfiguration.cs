using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Exam.Entities;

namespace NetDream.Modules.Exam.Migrations;
public class CourseLinkEntityTypeConfiguration : IEntityTypeConfiguration<CourseLinkEntity>
{
    public void Configure(EntityTypeBuilder<CourseLinkEntity> builder)
    {
        builder.ToTable("exam_course_link", table => table.HasComment("科目关联表"));
        builder.Property(table => table.CourseId).HasColumnName("course_id").HasMaxLength(10);
        builder.Property(table => table.LinkId).HasColumnName("link_id").HasMaxLength(10);
        builder.Property(table => table.Title).HasColumnName("title").HasMaxLength(100);
        
    }
}