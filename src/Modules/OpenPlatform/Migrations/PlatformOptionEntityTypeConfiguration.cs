using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.OpenPlatform.Entities;

namespace NetDream.Modules.OpenPlatform.Migrations
{
    public class PlatformOptionEntityTypeConfiguration : IEntityTypeConfiguration<PlatformOptionEntity>
    {
        public void Configure(EntityTypeBuilder<PlatformOptionEntity> builder)
        {
            builder.ToTable("open_platform_option", table => table.HasComment("ƽ̨һЩ�������ӿ�����"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.PlatformId).HasColumnName("platform_id");
            builder.Property(table => table.Store).HasColumnName("store").HasMaxLength(20).HasComment("ƽ̨����");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(30).HasComment("�ֶ�");
            builder.Property(table => table.Value).HasColumnName("value").HasComment("����ֵ");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
