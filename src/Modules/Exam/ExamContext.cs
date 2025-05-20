using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Exam.Entities;
using NetDream.Modules.Exam.Migrations;

namespace NetDream.Modules.Exam;
public class ExamContext(DbContextOptions<ExamContext> options) : DbContext(options)
{
    public DbSet<CourseEntity> Course { get; set; }
    public DbSet<CourseGradeEntity> CourseGrade { get; set; }
    public DbSet<CourseLinkEntity> CourseLink { get; set; }
    public DbSet<PageEntity> Page { get; set; }
    public DbSet<PageEvaluateEntity> PageEvaluate { get; set; }
    public DbSet<PageQuestionEntity> PageQuestion { get; set; }
    public DbSet<QuestionEntity> Question { get; set; }
    public DbSet<QuestionAnalysisEntity> QuestionAnalysis { get; set; }
    public DbSet<QuestionAnswerEntity> QuestionAnswer { get; set; }
    public DbSet<QuestionMaterialEntity> QuestionMaterial { get; set; }
    public DbSet<QuestionOptionEntity> QuestionOption { get; set; }
    public DbSet<QuestionWrongEntity> QuestionWrong { get; set; }
    public DbSet<UpgradeEntity> Upgrade { get; set; }
    public DbSet<UpgradePathEntity> UpgradePath { get; set; }
    public DbSet<UpgradeUserEntity> UpgradeUser { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CourseEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CourseGradeEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CourseLinkEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new PageEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new PageEvaluateEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new PageQuestionEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new QuestionEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new QuestionAnalysisEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new QuestionAnswerEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new QuestionMaterialEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new QuestionOptionEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new QuestionWrongEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new UpgradeEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new UpgradePathEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new UpgradeUserEntityTypeConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}