using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Catering.Entities;

namespace NetDream.Modules.Catering.Migrations;
public class GoodsEntityTypeConfiguration : IEntityTypeConfiguration<GoodsEntity>
{
    public void Configure(EntityTypeBuilder<GoodsEntity> builder)
    {
        builder.ToTable("eat_goods", table => table.HasComment(""));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.StoreId).HasColumnName("store_id").HasMaxLength(10);
        builder.Property(table => table.CatId).HasColumnName("cat_id").HasMaxLength(10);
        builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(255);
        builder.Property(table => table.Image).HasColumnName("image").HasMaxLength(255);
        builder.Property(table => table.RecipeId).HasColumnName("recipe_id").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.Description).HasColumnName("description").HasMaxLength(255);
        builder.Property(table => table.UpdatedAt).HasColumnName("updated_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasMaxLength(10).HasDefaultValue(0);
        
    }
}