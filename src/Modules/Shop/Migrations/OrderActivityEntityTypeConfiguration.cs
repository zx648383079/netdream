using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Migrations
{
    public class OrderActivityEntityTypeConfiguration : IEntityTypeConfiguration<OrderActivityEntity>
    {
        public void Configure(EntityTypeBuilder<OrderActivityEntity> builder)
        {
            builder.ToTable("OrderActivity", table => table.HasComment(""));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.OrderId).HasColumnName("order_id");
            builder.Property(table => table.ProductId).HasColumnName("product_id").HasDefaultValue(0);
            builder.Property(table => table.Type).HasColumnName("type");
            builder.Property(table => table.Amount).HasColumnName("amount").HasMaxLength(8).HasDefaultValue(0);
            builder.Property(table => table.Tag).HasColumnName("tag").HasDefaultValue(string.Empty);
            builder.Property(table => table.Name).HasColumnName("name").HasDefaultValue(string.Empty);
        }
    }
}
