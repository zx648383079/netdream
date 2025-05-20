using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Career.Entities;

namespace NetDream.Modules.Career.Migrations;
public class CompanyHrEntityTypeConfiguration : IEntityTypeConfiguration<CompanyHrEntity>
{
    public void Configure(EntityTypeBuilder<CompanyHrEntity> builder)
    {
        builder.ToTable("career_company_hr", table => table.HasComment("公司HR"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10).HasComment("user.id");
        builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(255);
        builder.Property(table => table.Avatar).HasColumnName("avatar").HasMaxLength(255);
        builder.Property(table => table.Address).HasColumnName("address").HasMaxLength(255).HasComment("地址");
        builder.Property(table => table.RegionId).HasColumnName("region_id").HasMaxLength(10).HasDefaultValue(0).HasComment("城市");
        builder.Property(table => table.Description).HasColumnName("description").HasMaxLength(255).HasComment("介绍");
        builder.Property(table => table.Credit).HasColumnName("credit").HasDefaultValue(5).HasComment("信用评分");
        builder.Property(table => table.Longitude).HasColumnName("longitude").HasMaxLength(50).HasComment("经度");
        builder.Property(table => table.Latitude).HasColumnName("latitude").HasMaxLength(50).HasComment("纬度");
        builder.Property(table => table.UpdatedAt).HasColumnName("updated_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasMaxLength(10).HasDefaultValue(0);
        
    }
}