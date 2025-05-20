using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Catering.Entities;

namespace NetDream.Modules.Catering.Migrations;
public class MaterialEntityTypeConfiguration : IEntityTypeConfiguration<MaterialEntity>
{
    public void Configure(EntityTypeBuilder<MaterialEntity> builder)
    {
        builder.ToTable("eat_material", table => table.HasComment("原材料"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(255);
        builder.Property(table => table.Image).HasColumnName("image").HasMaxLength(255);
        builder.Property(table => table.Description).HasColumnName("description").HasMaxLength(255);
        
    }
}