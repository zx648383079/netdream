using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Catering.Entities;

namespace NetDream.Modules.Catering.Migrations;
public class RecipeMaterialEntityTypeConfiguration : IEntityTypeConfiguration<RecipeMaterialEntity>
{
    public void Configure(EntityTypeBuilder<RecipeMaterialEntity> builder)
    {
        builder.ToTable("eat_recipe_material", table => table.HasComment("食谱的配方"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.RecipeId).HasColumnName("recipe_id").HasMaxLength(10);
        builder.Property(table => table.MaterialId).HasColumnName("material_id").HasMaxLength(10);
        builder.Property(table => table.Amount).HasColumnName("amount").HasMaxLength(8);
        builder.Property(table => table.Unit).HasColumnName("unit");
        
    }
}