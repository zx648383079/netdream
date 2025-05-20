using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Catering.Entities;

namespace NetDream.Modules.Catering.Migrations;
public class MaterialUnitEntityTypeConfiguration : IEntityTypeConfiguration<MaterialUnitEntity>
{
    public void Configure(EntityTypeBuilder<MaterialUnitEntity> builder)
    {
        builder.ToTable("eat_material_unit", table => table.HasComment("原材料单位换算"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.MaterialId).HasColumnName("material_id").HasMaxLength(255);
        builder.Property(table => table.FromAmount).HasColumnName("from_amount").HasMaxLength(8);
        builder.Property(table => table.FromUnit).HasColumnName("from_unit");
        builder.Property(table => table.ToAmount).HasColumnName("to_amount").HasMaxLength(8);
        builder.Property(table => table.ToUnit).HasColumnName("to_unit");
        
    }
}