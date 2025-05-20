using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Legwork.Entities;

namespace NetDream.Modules.Legwork.Migrations;
public class CategoryProviderEntityTypeConfiguration : IEntityTypeConfiguration<CategoryProviderEntity>
{
    public void Configure(EntityTypeBuilder<CategoryProviderEntity> builder)
    {
        builder.ToTable("leg_category_provider", table => table.HasComment(""));
        builder.Property(table => table.UserId).HasColumnName("user_id").HasMaxLength(10);
        builder.Property(table => table.CatId).HasColumnName("cat_id").HasMaxLength(10);
        builder.Property(table => table.Status).HasColumnName("status").HasDefaultValue(0);
        
    }
}