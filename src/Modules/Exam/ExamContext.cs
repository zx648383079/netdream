using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Exam.Entities;
using NetDream.Modules.Exam.Migrations;

namespace NetDream.Modules.Exam;
public class ExamContext(DbContextOptions<ExamContext> options) : DbContext(options)
{
    public DbSet<CourseEntity> Courses { get; set; }
    public DbSet<CourseGradeEntity> CourseGrades { get; set; }
    public DbSet<CourseLinkEntity> CourseLinks { get; set; }
    public DbSet<PageEntity> Pages { get; set; }
    public DbSet<PageEvaluateEntity> PageEvaluates { get; set; }
    public DbSet<PageQuestionEntity> PageQuestions { get; set; }
    public DbSet<QuestionEntity> Questions { get; set; }
    public DbSet<QuestionAnalysisEntity> QuestionAnalysis { get; set; }
    public DbSet<QuestionAnswerEntity> QuestionAnswers { get; set; }
    public DbSet<QuestionMaterialEntity> QuestionMaterials { get; set; }
    public DbSet<QuestionOptionEntity> QuestionOptions { get; set; }
    public DbSet<QuestionWrongEntity> QuestionWrongs { get; set; }
    public DbSet<UpgradeEntity> Upgrades { get; set; }
    public DbSet<UpgradePathEntity> UpgradePaths { get; set; }
    public DbSet<UpgradeUserEntity> UpgradeUsers { get; set; }
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