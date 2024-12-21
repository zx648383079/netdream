using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Auth.Entities;

namespace NetDream.Modules.Auth.Migrations
{
    public class BanAccountEntityTypeConfiguration : IEntityTypeConfiguration<BanAccountEntity>
    {
        public void Configure(EntityTypeBuilder<BanAccountEntity> builder)
        {
            builder.ToTable("user_ban_account", table => table.HasComment(""));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.UserId).HasColumnName("user_id").HasDefaultValue(0);
            builder.Property(table => table.ItemKey).HasColumnName("item_key").HasMaxLength(100);
            builder.Property(table => table.ItemType).HasColumnName("item_type").HasMaxLength(2).HasDefaultValue(0);
            builder.Property(table => table.PlatformId).HasColumnName("platform_id").HasDefaultValue(0).HasComment("ƽ̨id");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
