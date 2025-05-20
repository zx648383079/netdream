using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Career.Entities;

namespace NetDream.Modules.Career.Migrations;
public class ProfileEntityTypeConfiguration : IEntityTypeConfiguration<ProfileEntity>
{
    public void Configure(EntityTypeBuilder<ProfileEntity> builder)
    {
        builder.ToTable("career_profile", table => table.HasComment("求职者基本信息"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10).HasComment("使用user.id");
        builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(20);
        builder.Property(table => table.Avatar).HasColumnName("avatar").HasMaxLength(255);
        builder.Property(table => table.RegionId).HasColumnName("region_id").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.PositionId).HasColumnName("position_id").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.Status).HasColumnName("status").HasDefaultValue(0);
        builder.Property(table => table.Salary).HasColumnName("salary").HasMaxLength(10).HasDefaultValue(0.00).HasComment("月薪");
        builder.Property(table => table.SalaryRule).HasColumnName("salary_rule").HasDefaultValue(0).HasComment("工资方式");
        builder.Property(table => table.Remark).HasColumnName("remark").HasMaxLength(255).HasComment("个人介绍");
        builder.Property(table => table.Longitude).HasColumnName("longitude").HasMaxLength(50).HasComment("经度");
        builder.Property(table => table.Latitude).HasColumnName("latitude").HasMaxLength(50).HasComment("纬度");
        builder.Property(table => table.UpdatedAt).HasColumnName("updated_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasMaxLength(10).HasDefaultValue(0);
        
    }
}