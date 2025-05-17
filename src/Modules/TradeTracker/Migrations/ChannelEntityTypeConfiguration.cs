using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.TradeTracker.Entities;

namespace NetDream.Modules.TradeTracker.Migrations
{
    public class ChannelEntityTypeConfiguration : IEntityTypeConfiguration<ChannelEntity>
    {
        public void Configure(EntityTypeBuilder<ChannelEntity> builder)
        {
            builder.ToTable("tt_channels", table => table.HasComment("渠道表"));
            builder.HasKey(table => table.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.ShortName).HasColumnName("short_name").HasMaxLength(20).HasDefaultValue(string.Empty);
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(40);
            builder.Property(table => table.SiteUrl).HasColumnName("site_url").HasDefaultValue(string.Empty);
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasComment("");
        }
    }
}
