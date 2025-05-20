using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Career.Entities;
using NetDream.Modules.Career.Migrations;

namespace NetDream.Modules.Career;
public class CareerContext(DbContextOptions<CareerContext> options) : DbContext(options)
{
    public DbSet<AwardEntity> Award { get; set; }
    public DbSet<CertificateEntity> Certificate { get; set; }
    public DbSet<CompanyEntity> Company { get; set; }
    public DbSet<CompanyHrEntity> CompanyHr { get; set; }
    public DbSet<EducationalExperienceEntity> EducationalExperience { get; set; }
    public DbSet<IndustryEntity> Industry { get; set; }
    public DbSet<InfluenceEntity> Influence { get; set; }
    public DbSet<InterviewEntity> Interview { get; set; }
    public DbSet<InterviewLogEntity> InterviewLog { get; set; }
    public DbSet<JobEntity> Job { get; set; }
    public DbSet<JobLogEntity> JobLog { get; set; }
    public DbSet<PortfolioEntity> Portfolio { get; set; }
    public DbSet<PositionEntity> Position { get; set; }
    public DbSet<ProfileEntity> Profile { get; set; }
    public DbSet<ResumeEntity> Resume { get; set; }
    public DbSet<ResumePositionEntity> ResumePosition { get; set; }
    public DbSet<ResumeRegionEntity> ResumeRegion { get; set; }
    public DbSet<SkillEntity> Skill { get; set; }
    public DbSet<WorkExperienceEntity> WorkExperience { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AwardEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CertificateEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CompanyEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CompanyHrEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new EducationalExperienceEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new IndustryEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new InfluenceEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new InterviewEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new InterviewLogEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new JobEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new JobLogEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new PortfolioEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new PositionEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ProfileEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ResumeEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ResumePositionEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ResumeRegionEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new SkillEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new WorkExperienceEntityTypeConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}