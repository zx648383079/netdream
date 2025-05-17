using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.TradeTracker.Entities;

namespace NetDream.Modules.TradeTracker.Migrations
{
    public class ChannelProductEntityTypeConfiguration : IEntityTypeConfiguration<ChannelProductEntity>
    {
        public void Configure(EntityTypeBuilder<ChannelProductEntity> builder)
        {
            builder.ToTable("tt_channel_product", table => table.HasComment("渠道货品表"));
            builder.HasKey(table => table.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.ProductId).HasColumnName("product_id");
            builder.Property(table => table.ChannelId).HasColumnName("channel_id");
            builder.Property(table => table.PlatformNo).HasColumnName("platform_no").HasMaxLength(40).HasDefaultValue(string.Empty);
            builder.Property(table => table.ExtraMeta).HasColumnName("extra_meta").HasDefaultValue(string.Empty);
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
