using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Career.Entities;

namespace NetDream.Modules.Career.Migrations;
public class PositionEntityTypeConfiguration : IEntityTypeConfiguration<PositionEntity>
{
    public void Configure(EntityTypeBuilder<PositionEntity> builder)
    {
        builder.ToTable("career_position", table => table.HasComment("职位"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(20);
        builder.Property(table => table.ParentId).HasColumnName("parent_id").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.Description).HasColumnName("description").HasMaxLength(255);
        
    }
}