using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Career.Entities;

namespace NetDream.Modules.Career.Migrations;
public class CompanyEntityTypeConfiguration : IEntityTypeConfiguration<CompanyEntity>
{
    public void Configure(EntityTypeBuilder<CompanyEntity> builder)
    {
        builder.ToTable("career_company", table => table.HasComment("公司"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.IndustryId).HasColumnName("industry_id").HasMaxLength(10);
        builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(255);
        builder.Property(table => table.Address).HasColumnName("address").HasMaxLength(255).HasComment("地址");
        builder.Property(table => table.RegionId).HasColumnName("region_id").HasMaxLength(10).HasDefaultValue(0).HasComment("城市");
        builder.Property(table => table.Description).HasColumnName("description").HasMaxLength(255).HasComment("介绍");
        builder.Property(table => table.Credit).HasColumnName("credit").HasComment("信用评分");
        builder.Property(table => table.Type).HasColumnName("type").HasDefaultValue(0).HasComment("公司类型");
        builder.Property(table => table.EmployeeCount).HasColumnName("employee_count").HasDefaultValue(0).HasComment("雇员数");
        builder.Property(table => table.FinancingStage).HasColumnName("financing_stage").HasDefaultValue(0).HasComment("融资阶段");
        builder.Property(table => table.UpdatedAt).HasColumnName("updated_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasMaxLength(10).HasDefaultValue(0);
        
    }
}