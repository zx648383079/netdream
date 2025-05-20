using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Catering.Entities;

namespace NetDream.Modules.Catering.Migrations;
public class MaterialPriceEntityTypeConfiguration : IEntityTypeConfiguration<MaterialPriceEntity>
{
    public void Configure(EntityTypeBuilder<MaterialPriceEntity> builder)
    {
        builder.ToTable("eat_material_price", table => table.HasComment("原材料参考价格"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.MaterialId).HasColumnName("material_id").HasMaxLength(255);
        builder.Property(table => table.Amount).HasColumnName("amount").HasMaxLength(8);
        builder.Property(table => table.Unit).HasColumnName("unit");
        builder.Property(table => table.Price).HasColumnName("price").HasMaxLength(8);
        builder.Property(table => table.UpdatedAt).HasColumnName("updated_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasMaxLength(10).HasDefaultValue(0);
        
    }
}