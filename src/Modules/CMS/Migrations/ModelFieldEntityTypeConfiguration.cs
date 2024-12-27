using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.CMS.Entities;

namespace NetDream.Modules.CMS.Migrations
{
    public class ModelFieldEntityTypeConfiguration : IEntityTypeConfiguration<ModelFieldEntity>
    {
        public void Configure(EntityTypeBuilder<ModelFieldEntity> builder)
        {
            builder.ToTable("ModelField", table => table.HasComment(""));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(100);
            builder.Property(table => table.Field).HasColumnName("field").HasMaxLength(100);
            builder.Property(table => table.ModelId).HasColumnName("model_id");
            builder.Property(table => table.Type).HasColumnName("type").HasMaxLength(20).HasDefaultValue("text");
            builder.Property(table => table.Length).HasColumnName("length").HasMaxLength(10).HasDefaultValue(0);
            builder.Property(table => table.Position).HasColumnName("position").HasMaxLength(2).HasDefaultValue(99);
            builder.Property(table => table.FormType).HasColumnName("form_type").HasMaxLength(2).HasDefaultValue(0);
            builder.Property(table => table.IsMain).HasColumnName("is_main").HasDefaultValue(0);
            builder.Property(table => table.IsRequired).HasColumnName("is_required").HasDefaultValue(1);
            builder.Property(table => table.IsSearch).HasColumnName("is_search").HasDefaultValue(0).HasComment("是否能搜索");
            builder.Property(table => table.IsDisable).HasColumnName("is_disable").HasDefaultValue(0).HasComment("禁用/启用");
            builder.Property(table => table.IsSystem).HasColumnName("is_system").HasDefaultValue(0)
                .HasComment("系统自带禁止删除");
            builder.Property(table => table.Match).HasColumnName("match").HasDefaultValue(string.Empty);
            builder.Property(table => table.TipMessage).HasColumnName("tip_message").HasDefaultValue(string.Empty);
            builder.Property(table => table.ErrorMessage).HasColumnName("error_message").HasDefaultValue(string.Empty);
            builder.Property(table => table.TabName).HasColumnName("tab_name").HasMaxLength(4).HasDefaultValue(string.Empty).HasComment("编辑组名");
            builder.Property(table => table.Setting).HasColumnName("setting");
        }
    }
}
