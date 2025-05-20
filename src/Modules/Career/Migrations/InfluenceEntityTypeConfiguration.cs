using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Career.Entities;

namespace NetDream.Modules.Career.Migrations;
public class InfluenceEntityTypeConfiguration : IEntityTypeConfiguration<InfluenceEntity>
{
    public void Configure(EntityTypeBuilder<InfluenceEntity> builder)
    {
        builder.ToTable("career_influence", table => table.HasComment("社区影响力"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.UserId).HasColumnName("user_id").HasMaxLength(10);
        builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(200);
        builder.Property(table => table.Link).HasColumnName("link").HasMaxLength(200);
        builder.Property(table => table.UpdatedAt).HasColumnName("updated_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasMaxLength(10).HasDefaultValue(0);
        
    }
}