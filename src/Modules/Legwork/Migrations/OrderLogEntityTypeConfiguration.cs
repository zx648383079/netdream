using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Legwork.Entities;

namespace NetDream.Modules.Legwork.Migrations;
public class OrderLogEntityTypeConfiguration : IEntityTypeConfiguration<OrderLogEntity>
{
    public void Configure(EntityTypeBuilder<OrderLogEntity> builder)
    {
        builder.ToTable("leg_order_log", table => table.HasComment(""));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.OrderId).HasColumnName("order_id").HasMaxLength(10);
        builder.Property(table => table.UserId).HasColumnName("user_id").HasMaxLength(10);
        builder.Property(table => table.Status).HasColumnName("status").HasDefaultValue(1);
        builder.Property(table => table.Remark).HasColumnName("remark").HasMaxLength(255);
        builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasMaxLength(10).HasDefaultValue(0);
        
    }
}