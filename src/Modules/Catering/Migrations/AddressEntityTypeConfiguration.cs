using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Catering.Entities;

namespace NetDream.Modules.Catering.Migrations;
public class AddressEntityTypeConfiguration : IEntityTypeConfiguration<AddressEntity>
{
    public void Configure(EntityTypeBuilder<AddressEntity> builder)
    {
        builder.ToTable("eat_address", table => table.HasComment(""));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(30);
        builder.Property(table => table.RegionId).HasColumnName("region_id").HasMaxLength(10);
        builder.Property(table => table.UserId).HasColumnName("user_id").HasMaxLength(10);
        builder.Property(table => table.Tel).HasColumnName("tel").HasMaxLength(11);
        builder.Property(table => table.Address).HasColumnName("address").HasMaxLength(255);
        builder.Property(table => table.Longitude).HasColumnName("longitude").HasMaxLength(50).HasComment("经度");
        builder.Property(table => table.Latitude).HasColumnName("latitude").HasMaxLength(50).HasComment("纬度");
        builder.Property(table => table.UpdatedAt).HasColumnName("updated_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasMaxLength(10).HasDefaultValue(0);
        
    }
}