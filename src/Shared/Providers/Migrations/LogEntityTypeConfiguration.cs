using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using NetDream.Shared.Providers.Entities;

namespace NetDream.Shared.Providers.Migrations
{
    public class LogEntityTypeConfiguration(string prefix) : IEntityTypeConfiguration<LogEntity>
    {
        public void Configure(EntityTypeBuilder<LogEntity> builder)
        {
            builder.ToTable(prefix + "_log", table => table.HasComment("记录"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.ItemType).HasColumnName("item_type").HasMaxLength(1).HasDefaultValue(0);
            builder.Property(table => table.ItemId).HasColumnName("item_id");
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.Action).HasColumnName("action");
            builder.Property(table => table.Data).HasColumnName("data");
            builder.Property(table => table.Ip).HasColumnName("ip");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
