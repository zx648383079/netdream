using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Catering.Entities;

namespace NetDream.Modules.Catering.Migrations;
public class StoreRoleEntityTypeConfiguration : IEntityTypeConfiguration<StoreRoleEntity>
{
    public void Configure(EntityTypeBuilder<StoreRoleEntity> builder)
    {
        builder.ToTable("eat_store_role", table => table.HasComment("店铺员工权限"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.StoreId).HasColumnName("store_id").HasMaxLength(10);
        builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(20);
        builder.Property(table => table.Description).HasColumnName("description").HasMaxLength(255);
        builder.Property(table => table.Action).HasColumnName("action").HasMaxLength(255);
        builder.Property(table => table.UpdatedAt).HasColumnName("updated_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasMaxLength(10).HasDefaultValue(0);
        
    }
}