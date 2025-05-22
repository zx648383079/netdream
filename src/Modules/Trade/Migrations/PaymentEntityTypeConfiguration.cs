using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Trade.Entities;

namespace NetDream.Modules.Trade.Migrations
{
    public class PaymentEntityTypeConfiguration : IEntityTypeConfiguration<PaymentEntity>
    {
        public void Configure(EntityTypeBuilder<PaymentEntity> builder)
        {
            builder.ToTable("trade_payment", table => table.HasComment("Ö§¸¶·½Ê½"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(30);
            builder.Property(table => table.Code).HasColumnName("code").HasMaxLength(30);
            builder.Property(table => table.Icon).HasColumnName("icon").HasMaxLength(100).HasDefaultValue(string.Empty);
            builder.Property(table => table.Description).HasColumnName("description").HasDefaultValue(string.Empty);
            builder.Property(table => table.Settings).HasColumnName("settings");
        }
    }
}
