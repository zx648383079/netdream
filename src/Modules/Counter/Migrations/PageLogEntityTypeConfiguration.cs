using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Counter.Entities;

namespace NetDream.Modules.Counter.Migrations
{
    public class PageLogEntityTypeConfiguration : IEntityTypeConfiguration<PageLogEntity>
    {
        public void Configure(EntityTypeBuilder<PageLogEntity> builder)
        {
            builder.ToTable("ctr_page_log", table => table.HasComment("Ò³Ãæ·ÃÎÊ¼ÇÂ¼"));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Url).HasColumnName("url");
            builder.Property(table => table.VisitCount).HasColumnName("visit_count").HasDefaultValue(0);
        }
    }
}
