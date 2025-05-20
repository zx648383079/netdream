using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Career.Entities;

namespace NetDream.Modules.Career.Migrations;
public class ResumeRegionEntityTypeConfiguration : IEntityTypeConfiguration<ResumeRegionEntity>
{
    public void Configure(EntityTypeBuilder<ResumeRegionEntity> builder)
    {
        builder.ToTable("career_resume_region", table => table.HasComment("求职岗位"));
        builder.Property(table => table.ResumeId).HasColumnName("resume_id").HasMaxLength(10);
        builder.Property(table => table.RegionId).HasColumnName("region_id").HasMaxLength(10);
        
    }
}