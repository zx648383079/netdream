using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.CMS.Entities;

namespace NetDream.Modules.CMS.Migrations
{
    public class RecycleBinEntityTypeConfiguration : IEntityTypeConfiguration<RecycleBinEntity>
    {
        public void Configure(EntityTypeBuilder<RecycleBinEntity> builder)
        {
            builder.ToTable("RecycleBin", table => table.HasComment(""));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.SiteId).HasColumnName("site_id").HasDefaultValue(0);
            builder.Property(table => table.ModelId).HasColumnName("model_id").HasDefaultValue(0);
            builder.Property(table => table.ItemType).HasColumnName("item_type").HasMaxLength(1).HasDefaultValue(0);
            builder.Property(table => table.ItemId).HasColumnName("item_id");
            builder.Property(table => table.UserId).HasColumnName("user_id").HasComment("É¾³ýÕß");
            builder.Property(table => table.Title).HasColumnName("title");
            builder.Property(table => table.Remark).HasColumnName("remark").HasDefaultValue(string.Empty);
            builder.Property(table => table.Data).HasColumnName("data");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
