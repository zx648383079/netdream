using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Legwork.Entities;

namespace NetDream.Modules.Legwork.Migrations;
public class WaiterEntityTypeConfiguration : IEntityTypeConfiguration<WaiterEntity>
{
    public void Configure(EntityTypeBuilder<WaiterEntity> builder)
    {
        builder.ToTable("leg_waiter", table => table.HasComment(""));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.UserId).HasColumnName("user_id").HasMaxLength(10);
        builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(100).HasComment("名称");
        builder.Property(table => table.Tel).HasColumnName("tel").HasMaxLength(30).HasComment("联系方式");
        builder.Property(table => table.Address).HasColumnName("address").HasMaxLength(255).HasComment("联系地址");
        builder.Property(table => table.Longitude).HasColumnName("longitude").HasMaxLength(50).HasComment("经度");
        builder.Property(table => table.Latitude).HasColumnName("latitude").HasMaxLength(50).HasComment("纬度");
        builder.Property(table => table.MaxService).HasColumnName("max_service").HasMaxLength(5).HasDefaultValue(1).HasComment("每次同时服务最大数量");
        builder.Property(table => table.OverallRating).HasColumnName("overall_rating").HasDefaultValue(5).HasComment("综合评分");
        builder.Property(table => table.Status).HasColumnName("status").HasDefaultValue(0);
        builder.Property(table => table.UpdatedAt).HasColumnName("updated_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasMaxLength(10).HasDefaultValue(0);
        
    }
}