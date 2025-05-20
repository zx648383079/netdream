using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Exam.Entities;

namespace NetDream.Modules.Exam.Migrations;
public class UpgradePathEntityTypeConfiguration : IEntityTypeConfiguration<UpgradePathEntity>
{
    public void Configure(EntityTypeBuilder<UpgradePathEntity> builder)
    {
        builder.ToTable("exam_upgrade_path", table => table.HasComment("晋级路线表"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.ItemType).HasColumnName("item_type").HasDefaultValue(0);
        builder.Property(table => table.ItemId).HasColumnName("item_id").HasMaxLength(10);
        builder.Property(table => table.CourseGrade).HasColumnName("course_grade").HasMaxLength(5).HasDefaultValue(1);
        
    }
}