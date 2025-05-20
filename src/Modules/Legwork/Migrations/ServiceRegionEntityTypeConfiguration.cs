using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Legwork.Entities;

namespace NetDream.Modules.Legwork.Migrations;
public class ServiceRegionEntityTypeConfiguration : IEntityTypeConfiguration<ServiceRegionEntity>
{
    public void Configure(EntityTypeBuilder<ServiceRegionEntity> builder)
    {
        builder.ToTable("leg_service_region", table => table.HasComment(""));
        builder.Property(table => table.ServiceId).HasColumnName("service_id").HasMaxLength(10);
        builder.Property(table => table.RegionId).HasColumnName("region_id").HasMaxLength(10);
        
    }
}