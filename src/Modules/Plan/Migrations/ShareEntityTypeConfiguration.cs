using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Plan.Entities;

namespace NetDream.Modules.Plan.Migrations;
public class ShareEntityTypeConfiguration : IEntityTypeConfiguration<ShareEntity>
{
    public void Configure(EntityTypeBuilder<ShareEntity> builder)
    {
        builder.ToTable("task_share", table => table.HasComment("任务分享"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.UserId).HasColumnName("user_id").HasMaxLength(10);
        builder.Property(table => table.TaskId).HasColumnName("task_id").HasMaxLength(10);
        builder.Property(table => table.ShareType).HasColumnName("share_type").HasDefaultValue(0);
        builder.Property(table => table.ShareRule).HasColumnName("share_rule").HasMaxLength(20);
        builder.Property(table => table.UpdatedAt).HasColumnName("updated_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasMaxLength(10).HasDefaultValue(0);
        
    }
}