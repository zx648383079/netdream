using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.ResourceStore.Entities;

namespace NetDream.Modules.ResourceStore.Migrations;
public class ResourceFileEntityTypeConfiguration : IEntityTypeConfiguration<ResourceFileEntity>
{
    public void Configure(EntityTypeBuilder<ResourceFileEntity> builder)
    {
        builder.ToTable("res_resource_file", table => table.HasComment(""));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.UserId).HasColumnName("user_id").HasMaxLength(10);
        builder.Property(table => table.ResId).HasColumnName("res_id").HasMaxLength(10);
        builder.Property(table => table.FileType).HasColumnName("file_type").HasDefaultValue(0).HasComment("本地文件/网盘/种子");
        builder.Property(table => table.File).HasColumnName("file").HasMaxLength(255);
        builder.Property(table => table.ClickCount).HasColumnName("click_count").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.UpdatedAt).HasColumnName("updated_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasMaxLength(10).HasDefaultValue(0);
        
    }
}