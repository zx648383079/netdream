using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Career.Entities;

namespace NetDream.Modules.Career.Migrations;
public class WorkExperienceEntityTypeConfiguration : IEntityTypeConfiguration<WorkExperienceEntity>
{
    public void Configure(EntityTypeBuilder<WorkExperienceEntity> builder)
    {
        builder.ToTable("career_work_experience", table => table.HasComment("工作经历"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.UserId).HasColumnName("user_id").HasMaxLength(10);
        builder.Property(table => table.Company).HasColumnName("company").HasMaxLength(200);
        builder.Property(table => table.Position).HasColumnName("position").HasMaxLength(255).HasComment("职位");
        builder.Property(table => table.StartAt).HasColumnName("start_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.EndAt).HasColumnName("end_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.Remark).HasColumnName("remark").HasMaxLength(255).HasComment("经验描述");
        builder.Property(table => table.Certificate).HasColumnName("certificate").HasMaxLength(255).HasComment("工作证明");
        builder.Property(table => table.UpdatedAt).HasColumnName("updated_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasMaxLength(10).HasDefaultValue(0);
        
    }
}