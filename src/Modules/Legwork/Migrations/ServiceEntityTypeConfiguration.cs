using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Legwork.Entities;

namespace NetDream.Modules.Legwork.Migrations;
public class ServiceEntityTypeConfiguration : IEntityTypeConfiguration<ServiceEntity>
{
    public void Configure(EntityTypeBuilder<ServiceEntity> builder)
    {
        builder.ToTable("leg_service", table => table.HasComment(""));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.UserId).HasColumnName("user_id").HasMaxLength(10);
        builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(100).HasComment("服务名");
        builder.Property(table => table.CatId).HasColumnName("cat_id").HasMaxLength(10).HasDefaultValue(0).HasComment("服务分类");
        builder.Property(table => table.Thumb).HasColumnName("thumb").HasMaxLength(200).HasComment("缩略图");
        builder.Property(table => table.Brief).HasColumnName("brief").HasMaxLength(255).HasComment("说明");
        builder.Property(table => table.Price).HasColumnName("price").HasMaxLength(8).HasDefaultValue(0.00);
        builder.Property(table => table.Content).HasColumnName("content").HasComment("内容");
        builder.Property(table => table.Form).HasColumnName("form").HasComment("表单设置");
        builder.Property(table => table.Status).HasColumnName("status").HasDefaultValue(0).HasComment("服务状态");
        builder.Property(table => table.UpdatedAt).HasColumnName("updated_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasMaxLength(10).HasDefaultValue(0);
        
    }
}