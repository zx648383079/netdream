using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Catering.Entities;

namespace NetDream.Modules.Catering.Migrations;
public class StorePatronEntityTypeConfiguration : IEntityTypeConfiguration<StorePatronEntity>
{
    public void Configure(EntityTypeBuilder<StorePatronEntity> builder)
    {
        builder.ToTable("eat_store_patron", table => table.HasComment("店铺顾客"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.StoreId).HasColumnName("store_id").HasMaxLength(10);
        builder.Property(table => table.UserId).HasColumnName("user_id").HasMaxLength(10);
        builder.Property(table => table.GroupId).HasColumnName("group_id").HasMaxLength(10);
        builder.Property(table => table.Amount).HasColumnName("amount").HasMaxLength(10).HasDefaultValue(0).HasComment("光顾次数");
        builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(20);
        builder.Property(table => table.Remark).HasColumnName("remark").HasMaxLength(255);
        builder.Property(table => table.UpdatedAt).HasColumnName("updated_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasMaxLength(10).HasDefaultValue(0);
        
    }
}