using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Catering.Entities;

namespace NetDream.Modules.Catering.Migrations;
public class PurchaseOrderEntityTypeConfiguration : IEntityTypeConfiguration<PurchaseOrderEntity>
{
    public void Configure(EntityTypeBuilder<PurchaseOrderEntity> builder)
    {
        builder.ToTable("eat_purchase_order", table => table.HasComment("采购单"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.StoreId).HasColumnName("store_id").HasMaxLength(10);
        builder.Property(table => table.UserId).HasColumnName("user_id").HasMaxLength(10);
        builder.Property(table => table.Status).HasColumnName("status").HasDefaultValue(0);
        builder.Property(table => table.Price).HasColumnName("price").HasMaxLength(8);
        builder.Property(table => table.Remark).HasColumnName("remark").HasMaxLength(255);
        builder.Property(table => table.ExecuteId).HasColumnName("execute_id").HasMaxLength(10).HasDefaultValue(0).HasComment("采购人");
        builder.Property(table => table.CheckId).HasColumnName("check_id").HasMaxLength(10).HasDefaultValue(0).HasComment("验收人");
        builder.Property(table => table.ExecuteAt).HasColumnName("execute_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.CheckAt).HasColumnName("check_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.UpdatedAt).HasColumnName("updated_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasMaxLength(10).HasDefaultValue(0);
        
    }
}