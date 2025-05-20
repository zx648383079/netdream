using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Career.Entities;

namespace NetDream.Modules.Career.Migrations;
public class AwardEntityTypeConfiguration : IEntityTypeConfiguration<AwardEntity>
{
    public void Configure(EntityTypeBuilder<AwardEntity> builder)
    {
        builder.ToTable("career_award", table => table.HasComment("个人获得的奖项"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.UserId).HasColumnName("user_id").HasMaxLength(10);
        builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(200);
        builder.Property(table => table.Remark).HasColumnName("remark").HasMaxLength(255).HasComment("奖项描述");
        builder.Property(table => table.GotAt).HasColumnName("got_at").HasMaxLength(10).HasDefaultValue(0).HasComment("获得的时间");
        builder.Property(table => table.UpdatedAt).HasColumnName("updated_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasMaxLength(10).HasDefaultValue(0);
        
    }
}