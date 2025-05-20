using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Career.Entities;

namespace NetDream.Modules.Career.Migrations;
public class SkillEntityTypeConfiguration : IEntityTypeConfiguration<SkillEntity>
{
    public void Configure(EntityTypeBuilder<SkillEntity> builder)
    {
        builder.ToTable("career_skill", table => table.HasComment("个人技能"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.UserId).HasColumnName("user_id").HasMaxLength(10);
        builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(200);
        builder.Property(table => table.Score).HasColumnName("score").HasDefaultValue(100).HasComment("自我评分%");
        builder.Property(table => table.UpdatedAt).HasColumnName("updated_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasMaxLength(10).HasDefaultValue(0);
        
    }
}