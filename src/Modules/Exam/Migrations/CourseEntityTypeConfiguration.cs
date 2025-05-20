using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Exam.Entities;

namespace NetDream.Modules.Exam.Migrations;
public class CourseEntityTypeConfiguration : IEntityTypeConfiguration<CourseEntity>
{
    public void Configure(EntityTypeBuilder<CourseEntity> builder)
    {
        builder.ToTable("exam_course", table => table.HasComment("科目"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(30);
        builder.Property(table => table.Thumb).HasColumnName("thumb").HasMaxLength(200);
        builder.Property(table => table.Description).HasColumnName("description").HasMaxLength(200);
        builder.Property(table => table.ParentId).HasColumnName("parent_id").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.UpdatedAt).HasColumnName("updated_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasMaxLength(10).HasDefaultValue(0);
        
    }
}