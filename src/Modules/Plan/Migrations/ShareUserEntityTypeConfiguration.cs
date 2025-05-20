using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Plan.Entities;

namespace NetDream.Modules.Plan.Migrations;
public class ShareUserEntityTypeConfiguration : IEntityTypeConfiguration<ShareUserEntity>
{
    public void Configure(EntityTypeBuilder<ShareUserEntity> builder)
    {
        builder.ToTable("task_share_user", table => table.HasComment("任务分享领取用户"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.UserId).HasColumnName("user_id").HasMaxLength(10);
        builder.Property(table => table.ShareId).HasColumnName("share_id").HasMaxLength(10);
        builder.Property(table => table.DeletedAt).HasColumnName("deleted_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasMaxLength(10).HasDefaultValue(0);
        
    }
}