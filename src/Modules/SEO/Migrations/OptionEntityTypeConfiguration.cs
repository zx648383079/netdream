using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.SEO.Entities;

namespace NetDream.Modules.SEO.Migrations
{
    public class OptionEntityTypeConfiguration : IEntityTypeConfiguration<OptionEntity>
    {
        public void Configure(EntityTypeBuilder<OptionEntity> builder)
        {
            builder.ToTable("seo_option", table => table.HasComment("全局设置"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(20);
            builder.Property(table => table.Code).HasColumnName("code").HasMaxLength(50).HasDefaultValue(string.Empty);
            builder.Property(table => table.ParentId).HasColumnName("parent_id").HasDefaultValue(0);
            builder.Property(table => table.Type).HasColumnName("type").HasMaxLength(20).HasDefaultValue("text");
            builder.Property(table => table.Visibility).HasColumnName("visibility").HasMaxLength(1).HasDefaultValue(1).HasComment("是否对外显示, 0 页面不可见，1 编辑可见 2 前台可见");
            builder.Property(table => table.DefaultValue).HasColumnName("default_value").HasDefaultValue(string.Empty).HasComment("默认值或候选值");
            builder.Property(table => table.Value).HasColumnName("value");
            builder.Property(table => table.Position).HasColumnName("position").HasMaxLength(2).HasDefaultValue(99);
        }
    }
}
