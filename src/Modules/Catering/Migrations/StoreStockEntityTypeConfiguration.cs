using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Catering.Entities;

namespace NetDream.Modules.Catering.Migrations;
public class StoreStockEntityTypeConfiguration : IEntityTypeConfiguration<StoreStockEntity>
{
    public void Configure(EntityTypeBuilder<StoreStockEntity> builder)
    {
        builder.ToTable("eat_store_stock", table => table.HasComment("店铺库存"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.StoreId).HasColumnName("store_id").HasMaxLength(10);
        builder.Property(table => table.CatId).HasColumnName("cat_id").HasMaxLength(10);
        builder.Property(table => table.MaterialId).HasColumnName("material_id").HasMaxLength(10);
        builder.Property(table => table.Amount).HasColumnName("amount").HasMaxLength(8);
        builder.Property(table => table.Unit).HasColumnName("unit");
        builder.Property(table => table.UpdatedAt).HasColumnName("updated_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasMaxLength(10).HasDefaultValue(0);
        
    }
}