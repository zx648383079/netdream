using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Career.Entities;

namespace NetDream.Modules.Career.Migrations;
public class ResumePositionEntityTypeConfiguration : IEntityTypeConfiguration<ResumePositionEntity>
{
    public void Configure(EntityTypeBuilder<ResumePositionEntity> builder)
    {
        builder.ToTable("career_resume_position", table => table.HasComment("求职岗位"));
        builder.Property(table => table.ResumeId).HasColumnName("resume_id").HasMaxLength(10);
        builder.Property(table => table.PositionId).HasColumnName("position_id").HasMaxLength(10);
        
    }
}