using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Migrations
{
    public class PluginEntityTypeConfiguration : IEntityTypeConfiguration<PluginEntity>
    {
        public void Configure(EntityTypeBuilder<PluginEntity> builder)
        {
            builder.ToTable("Plugin", table => table.HasComment("����б������ļ�"));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Code).HasColumnName("code").HasMaxLength(20).HasComment("�������");
            builder.Property(table => table.Setting).HasColumnName("setting").HasComment("������Ϣ");
            builder.Property(table => table.Status).HasColumnName("status").HasDefaultValue(0).HasComment("��ʼ״̬");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
