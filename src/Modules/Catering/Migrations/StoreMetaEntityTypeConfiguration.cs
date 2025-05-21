using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Catering.Entities;

namespace NetDream.Modules.Catering.Migrations;
public class StoreMetaEntityTypeConfiguration : IEntityTypeConfiguration<StoreMetaEntity>
{
    public void Configure(EntityTypeBuilder<StoreMetaEntity> builder)
    {
        builder.ToTable("eat_store_meta", table => table.HasComment(""));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.ItemId).HasColumnName("item_id").HasMaxLength(10);
        builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(100);
        builder.Property(table => table.Content).HasColumnName("content");
        
    }
}