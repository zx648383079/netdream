using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Catering.Entities;

namespace NetDream.Modules.Catering.Migrations;
public class StorePatronGroupEntityTypeConfiguration : IEntityTypeConfiguration<StorePatronGroupEntity>
{
    public void Configure(EntityTypeBuilder<StorePatronGroupEntity> builder)
    {
        builder.ToTable("eat_store_patron_group", table => table.HasComment("店铺顾客分组"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.StoreId).HasColumnName("store_id").HasMaxLength(10);
        builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(20);
        builder.Property(table => table.Remark).HasColumnName("remark").HasMaxLength(255);
        builder.Property(table => table.Discount).HasColumnName("discount").HasDefaultValue(100);
        
    }
}