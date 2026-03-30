using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Counter.Entities;

namespace NetDream.Modules.Counter.Migrations
{
    public class DayLogEntityTypeConfiguration : IEntityTypeConfiguration<DayLogEntity>
    {
        public void Configure(EntityTypeBuilder<DayLogEntity> builder)
        {
            builder.ToTable("log_day", table => table.HasComment("按日期统计"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.HappenDay).HasColumnName("happen_day").HasMaxLength(20);
            builder.Property(table => table.ItemType).HasColumnName("item_type").HasMaxLength(1).HasDefaultValue(0);
            builder.Property(table => table.ItemId).HasColumnName("item_id");
            builder.Property(table => table.Action).HasColumnName("action").HasDefaultValue(0);
            builder.Property(table => table.HappenCount).HasColumnName("happen_count").HasDefaultValue(0);
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
