using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Migrations
{
    public class ShippingGroupEntityTypeConfiguration : IEntityTypeConfiguration<ShippingGroupEntity>
    {
        public void Configure(EntityTypeBuilder<ShippingGroupEntity> builder)
        {
            builder.ToTable("ShippingGroup", table => table.HasComment(""));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.ShippingId).HasColumnName("shipping_id");
            builder.Property(table => table.IsAll).HasColumnName("is_all").HasDefaultValue(0);
            builder.Property(table => table.FirstStep).HasColumnName("first_step").HasMaxLength(10).HasDefaultValue(0);
            builder.Property(table => table.FirstFee).HasColumnName("first_fee").HasMaxLength(10).HasDefaultValue(0);
            builder.Property(table => table.Additional).HasColumnName("additional").HasMaxLength(10).HasDefaultValue(0);
            builder.Property(table => table.AdditionalFee).HasColumnName("additional_fee").HasMaxLength(10).HasDefaultValue(0);
            builder.Property(table => table.FreeStep).HasColumnName("free_step").HasMaxLength(10).HasDefaultValue(0);
        }
    }
}
