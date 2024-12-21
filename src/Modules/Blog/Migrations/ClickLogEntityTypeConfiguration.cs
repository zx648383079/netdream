using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Blog.Entities;

namespace NetDream.Modules.Blog.Migrations
{
    public class ClickLogEntityTypeConfiguration : IEntityTypeConfiguration<ClickLogEntity>
    {
        public void Configure(EntityTypeBuilder<ClickLogEntity> builder)
        {
            builder.ToTable("blog_click_log", table => table.HasComment(""));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.HappenDay).HasColumnName("happen_day");
            builder.Property(table => table.BlogId).HasColumnName("blog_id");
            builder.Property(table => table.ClickCount).HasColumnName("click_count").HasDefaultValue(0);
        }
    }
}
