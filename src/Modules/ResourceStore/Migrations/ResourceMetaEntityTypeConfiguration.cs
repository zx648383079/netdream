using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.ResourceStore.Entities;

namespace NetDream.Modules.ResourceStore.Migrations;
public class ResourceMetaEntityTypeConfiguration : IEntityTypeConfiguration<ResourceMetaEntity>
{
    public void Configure(EntityTypeBuilder<ResourceMetaEntity> builder)
    {
        builder.ToTable("res_resource_meta", table => table.HasComment(""));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.ResId).HasColumnName("res_id").HasMaxLength(10);
        builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(40);
        builder.Property(table => table.Content).HasColumnName("content");
        
    }
}