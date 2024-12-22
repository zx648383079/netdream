using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using NetDream.Shared.Providers.Entities;

namespace NetDream.Shared.Providers.Migrations
{
    public class SketchLogEntityMigration
        : IEntityTypeConfiguration<SketchLogEntity>
    {
        public void Configure(EntityTypeBuilder<SketchLogEntity> builder)
        {
            builder.ToTable("base_sketch_box", table => table.HasComment("草稿记录"));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.ItemType).HasColumnName("item_type").HasMaxLength(1).HasDefaultValue(0);
            builder.Property(table => table.ItemId).HasColumnName("item_id");
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.Data).HasColumnName("data");
            builder.Property(table => table.Ip).HasColumnName("ip");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
