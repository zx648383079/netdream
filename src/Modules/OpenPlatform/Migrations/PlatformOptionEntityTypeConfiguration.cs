using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.OpenPlatform.Entities;

namespace NetDream.Modules.OpenPlatform.Migrations
{
    public class PlatformOptionEntityTypeConfiguration : IEntityTypeConfiguration<PlatformOptionEntity>
    {
        public void Configure(EntityTypeBuilder<PlatformOptionEntity> builder)
        {
            builder.ToTable("open_platform_option", table => table.HasComment("平台一些第三方接口配置"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.PlatformId).HasColumnName("platform_id");
            builder.Property(table => table.Store).HasColumnName("store").HasMaxLength(20).HasComment("平台别名");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(30).HasComment("字段");
            builder.Property(table => table.Value).HasColumnName("value").HasComment("配置值");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
