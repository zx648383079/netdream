using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Migrations
{
    public class OrderLogEntityTypeConfiguration : IEntityTypeConfiguration<OrderLogEntity>
    {
        public void Configure(EntityTypeBuilder<OrderLogEntity> builder)
        {
            builder.ToTable("OrderLog", table => table.HasComment(""));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.OrderId).HasColumnName("order_id");
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.Status).HasColumnName("status").HasDefaultValue(1);
            builder.Property(table => table.Remark).HasColumnName("remark").HasDefaultValue(string.Empty);
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
