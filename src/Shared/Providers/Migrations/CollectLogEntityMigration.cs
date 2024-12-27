using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using NetDream.Shared.Providers.Entities;

namespace NetDream.Shared.Providers.Migrations
{
    public class CollectLogEntityMigration(string prefix) 
        : IEntityTypeConfiguration<CollectLogEntity>
    {
        public void Configure(EntityTypeBuilder<CollectLogEntity> builder)
        {
            builder.ToTable(prefix + "_collect", table => table.HasComment("收藏记录"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.ItemType).HasColumnName("item_type").HasMaxLength(1).HasDefaultValue(0);
            builder.Property(table => table.ItemId).HasColumnName("item_id");
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.ExtraData).HasColumnName("extra_data");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
