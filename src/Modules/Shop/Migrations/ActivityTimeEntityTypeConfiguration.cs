using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Migrations
{
    public class ActivityTimeEntityTypeConfiguration : IEntityTypeConfiguration<ActivityTimeEntity>
    {
        public void Configure(EntityTypeBuilder<ActivityTimeEntity> builder)
        {
            builder.ToTable("ActivityTime", table => table.HasComment(""));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Title).HasColumnName("title").HasMaxLength(40);
            builder.Property(table => table.StartAt).HasColumnName("start_at");
            builder.Property(table => table.EndAt).HasColumnName("end_at");
        }
    }
}
