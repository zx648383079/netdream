using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.SEO.Entities;

namespace NetDream.Modules.SEO.Migrations
{
    public class OptionEntityTypeConfiguration : IEntityTypeConfiguration<OptionEntity>
    {
        public void Configure(EntityTypeBuilder<OptionEntity> builder)
        {
            builder.ToTable("seo_option", table => table.HasComment("ȫ������"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(20);
            builder.Property(table => table.Code).HasColumnName("code").HasMaxLength(50).HasDefaultValue(string.Empty);
            builder.Property(table => table.ParentId).HasColumnName("parent_id").HasDefaultValue(0);
            builder.Property(table => table.Type).HasColumnName("type").HasMaxLength(20).HasDefaultValue("text");
            builder.Property(table => table.Visibility).HasColumnName("visibility").HasMaxLength(1).HasDefaultValue(1).HasComment("�Ƿ������ʾ, 0 ҳ�治�ɼ���1 �༭�ɼ� 2 ǰ̨�ɼ�");
            builder.Property(table => table.DefaultValue).HasColumnName("default_value").HasDefaultValue(string.Empty).HasComment("Ĭ��ֵ���ѡֵ");
            builder.Property(table => table.Value).HasColumnName("value");
            builder.Property(table => table.Position).HasColumnName("position").HasMaxLength(2).HasDefaultValue(99);
        }
    }
}
