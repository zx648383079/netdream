using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Counter.Entities;

namespace NetDream.Modules.Counter.Migrations
{
    public class SiteEntityTypeConfiguration : IEntityTypeConfiguration<SiteEntity>
    {
        public void Configure(EntityTypeBuilder<SiteEntity> builder)
        {
            builder.ToTable("ctr_site", table => table.HasComment("·ÖÎöÕ¾µã"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Name).HasColumnName("name");
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.UpdatedAt).HasColumnName("created_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
