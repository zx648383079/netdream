using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Legwork.Entities;

namespace NetDream.Modules.Legwork.Migrations;
public class ServiceWaiterEntityTypeConfiguration : IEntityTypeConfiguration<ServiceWaiterEntity>
{
    public void Configure(EntityTypeBuilder<ServiceWaiterEntity> builder)
    {
        builder.ToTable("leg_service_waiter", table => table.HasComment(""));
        builder.Property(table => table.ServiceId).HasColumnName("service_id").HasMaxLength(10);
        builder.Property(table => table.UserId).HasColumnName("user_id").HasMaxLength(10);
        builder.Property(table => table.Status).HasColumnName("status").HasDefaultValue(0);
        
    }
}