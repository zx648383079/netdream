using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Career.Entities;

namespace NetDream.Modules.Career.Migrations;
public class EducationalExperienceEntityTypeConfiguration : IEntityTypeConfiguration<EducationalExperienceEntity>
{
    public void Configure(EntityTypeBuilder<EducationalExperienceEntity> builder)
    {
        builder.ToTable("career_educational_experience", table => table.HasComment("教育经历"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.UserId).HasColumnName("user_id").HasMaxLength(10);
        builder.Property(table => table.School).HasColumnName("school").HasMaxLength(200);
        builder.Property(table => table.Major).HasColumnName("major").HasMaxLength(255).HasComment("专业");
        builder.Property(table => table.Education).HasColumnName("education").HasComment("学历");
        builder.Property(table => table.StartAt).HasColumnName("start_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.EndAt).HasColumnName("end_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.Remark).HasColumnName("remark").HasMaxLength(255).HasComment("经验描述");
        builder.Property(table => table.Certificate).HasColumnName("certificate").HasMaxLength(255).HasComment("学历证明");
        builder.Property(table => table.UpdatedAt).HasColumnName("updated_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasMaxLength(10).HasDefaultValue(0);
        
    }
}