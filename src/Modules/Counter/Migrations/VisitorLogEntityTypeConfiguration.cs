using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Counter.Entities;

namespace NetDream.Modules.Counter.Migrations
{
    public class VisitorLogEntityTypeConfiguration : IEntityTypeConfiguration<VisitorLogEntity>
    {
        public void Configure(EntityTypeBuilder<VisitorLogEntity> builder)
        {
            builder.ToTable("ctr_visitor_log", table => table.HasComment("·Ã¿ÍÈÕÖ¾"));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.UserId).HasColumnName("user_id").HasDefaultValue(0);
            builder.Property(table => table.Ip).HasColumnName("ip").HasMaxLength(120);
            builder.Property(table => table.FirstAt).HasColumnName("first_at");
            builder.Property(table => table.LastAt).HasColumnName("last_at");
        }
    }
}
