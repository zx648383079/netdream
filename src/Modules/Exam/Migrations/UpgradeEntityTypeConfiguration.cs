using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Exam.Entities;

namespace NetDream.Modules.Exam.Migrations;
public class UpgradeEntityTypeConfiguration : IEntityTypeConfiguration<UpgradeEntity>
{
    public void Configure(EntityTypeBuilder<UpgradeEntity> builder)
    {
        builder.ToTable("exam_upgrade", table => table.HasComment("晋级名称表"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.CourseId).HasColumnName("course_id").HasMaxLength(10).HasComment("所属科目");
        builder.Property(table => table.CourseGrade).HasColumnName("course_grade").HasMaxLength(5).HasDefaultValue(1);
        builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(100);
        builder.Property(table => table.Icon).HasColumnName("icon").HasMaxLength(100).HasComment("勋章图标");
        builder.Property(table => table.Description).HasColumnName("description").HasMaxLength(255);
        builder.Property(table => table.UpdatedAt).HasColumnName("updated_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasMaxLength(10).HasDefaultValue(0);
        
    }
}