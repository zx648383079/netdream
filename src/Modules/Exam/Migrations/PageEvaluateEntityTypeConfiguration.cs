using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Exam.Entities;

namespace NetDream.Modules.Exam.Migrations;
public class PageEvaluateEntityTypeConfiguration : IEntityTypeConfiguration<PageEvaluateEntity>
{
    public void Configure(EntityTypeBuilder<PageEvaluateEntity> builder)
    {
        builder.ToTable("exam_page_evaluate", table => table.HasComment("试卷评估结果"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.PageId).HasColumnName("page_id").HasMaxLength(10);
        builder.Property(table => table.UserId).HasColumnName("user_id").HasMaxLength(10);
        builder.Property(table => table.SpentTime).HasColumnName("spent_time").HasMaxLength(5).HasDefaultValue(0);
        builder.Property(table => table.Right).HasColumnName("right").HasMaxLength(5).HasDefaultValue(0).HasComment("正确数量");
        builder.Property(table => table.Wrong).HasColumnName("wrong").HasMaxLength(5).HasDefaultValue(0).HasComment("错误数量");
        builder.Property(table => table.Score).HasColumnName("score").HasMaxLength(5).HasDefaultValue(0);
        builder.Property(table => table.Status).HasColumnName("status").HasDefaultValue(0).HasComment("状态");
        builder.Property(table => table.MarkerId).HasColumnName("marker_id").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.Remark).HasColumnName("remark").HasMaxLength(255).HasComment("评语");
        builder.Property(table => table.UpdatedAt).HasColumnName("updated_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasMaxLength(10).HasDefaultValue(0);
        
    }
}