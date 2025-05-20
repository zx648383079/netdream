using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Career.Entities;

namespace NetDream.Modules.Career.Migrations;
public class IndustryEntityTypeConfiguration : IEntityTypeConfiguration<IndustryEntity>
{
    public void Configure(EntityTypeBuilder<IndustryEntity> builder)
    {
        builder.ToTable("career_industry", table => table.HasComment("行业"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(20);
        
    }
}