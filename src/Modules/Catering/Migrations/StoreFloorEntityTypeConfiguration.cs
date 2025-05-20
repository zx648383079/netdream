using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Catering.Entities;

namespace NetDream.Modules.Catering.Migrations;
public class StoreFloorEntityTypeConfiguration : IEntityTypeConfiguration<StoreFloorEntity>
{
    public void Configure(EntityTypeBuilder<StoreFloorEntity> builder)
    {
        builder.ToTable("eat_store_floor", table => table.HasComment("店铺房间"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.StoreId).HasColumnName("store_id").HasMaxLength(10);
        builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(20);
        builder.Property(table => table.Map).HasColumnName("map");
        
    }
}