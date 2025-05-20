using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Career.Entities;

namespace NetDream.Modules.Career.Migrations;
public class PortfolioEntityTypeConfiguration : IEntityTypeConfiguration<PortfolioEntity>
{
    public void Configure(EntityTypeBuilder<PortfolioEntity> builder)
    {
        builder.ToTable("career_portfolio", table => table.HasComment("项目作品"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.UserId).HasColumnName("user_id").HasMaxLength(10);
        builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(200);
        builder.Property(table => table.Link).HasColumnName("link").HasMaxLength(200);
        builder.Property(table => table.Trade).HasColumnName("trade").HasMaxLength(200).HasComment("行业");
        builder.Property(table => table.Function).HasColumnName("function").HasMaxLength(255).HasComment("功能");
        builder.Property(table => table.Remark).HasColumnName("remark").HasMaxLength(255).HasComment("描述");
        builder.Property(table => table.Duty).HasColumnName("duty").HasMaxLength(255).HasComment("职责");
        builder.Property(table => table.Images).HasColumnName("images").HasMaxLength(255).HasComment("作品图片");
        builder.Property(table => table.UpdatedAt).HasColumnName("updated_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasMaxLength(10).HasDefaultValue(0);
        
    }
}