using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Plan.Entities;

namespace NetDream.Modules.Plan.Migrations;
public class CommentEntityTypeConfiguration : IEntityTypeConfiguration<CommentEntity>
{
    public void Configure(EntityTypeBuilder<CommentEntity> builder)
    {
        builder.ToTable("task_comment", table => table.HasComment("任务执行评论"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.UserId).HasColumnName("user_id").HasMaxLength(10);
        builder.Property(table => table.TaskId).HasColumnName("task_id").HasMaxLength(10);
        builder.Property(table => table.LogId).HasColumnName("log_id").HasMaxLength(10).HasDefaultValue(0).HasComment("关联执行记录");
        builder.Property(table => table.Content).HasColumnName("content").HasMaxLength(255);
        builder.Property(table => table.Type).HasColumnName("type").HasDefaultValue(0);
        builder.Property(table => table.Status).HasColumnName("status").HasDefaultValue(5);
        builder.Property(table => table.UpdatedAt).HasColumnName("updated_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasMaxLength(10).HasDefaultValue(0);
        
    }
}