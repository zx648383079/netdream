using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Exam.Entities;

namespace NetDream.Modules.Exam.Migrations;
public class UpgradeUserEntityTypeConfiguration : IEntityTypeConfiguration<UpgradeUserEntity>
{
    public void Configure(EntityTypeBuilder<UpgradeUserEntity> builder)
    {
        builder.ToTable("exam_upgrade_user", table => table.HasComment("用户晋级记录表"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.UpgradeId).HasColumnName("upgrade_id").HasMaxLength(10);
        builder.Property(table => table.UserId).HasColumnName("user_id").HasMaxLength(10);
        builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasMaxLength(10).HasDefaultValue(0);
        
    }
}