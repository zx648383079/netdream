using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Migrations
{
    public class PluginEntityTypeConfiguration : IEntityTypeConfiguration<PluginEntity>
    {
        public void Configure(EntityTypeBuilder<PluginEntity> builder)
        {
            builder.ToTable("Plugin", table => table.HasComment("插件列表及配置文件"));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Code).HasColumnName("code").HasMaxLength(20).HasComment("插件别名");
            builder.Property(table => table.Setting).HasColumnName("setting").HasComment("配置信息");
            builder.Property(table => table.Status).HasColumnName("status").HasDefaultValue(0).HasComment("开始状态");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
