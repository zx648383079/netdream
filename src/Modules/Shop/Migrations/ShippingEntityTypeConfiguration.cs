using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Migrations
{
    public class ShippingEntityTypeConfiguration : IEntityTypeConfiguration<ShippingEntity>
    {
        public void Configure(EntityTypeBuilder<ShippingEntity> builder)
        {
            builder.ToTable("Shipping", table => table.HasComment(""));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(30);
            builder.Property(table => table.Code).HasColumnName("code").HasMaxLength(30);
            builder.Property(table => table.Method).HasColumnName("method").HasMaxLength(2).HasDefaultValue(0).HasComment("计费方式");
            builder.Property(table => table.Icon).HasColumnName("icon").HasMaxLength(100).HasDefaultValue(string.Empty);
            builder.Property(table => table.Description).HasColumnName("description").HasDefaultValue(string.Empty);
            builder.Property(table => table.Position).HasColumnName("position").HasMaxLength(2).HasDefaultValue(50);
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
