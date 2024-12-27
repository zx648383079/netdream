using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Migrations
{
    public class AttributeEntityTypeConfiguration : IEntityTypeConfiguration<AttributeEntity>
    {
        public void Configure(EntityTypeBuilder<AttributeEntity> builder)
        {
            builder.ToTable("shop_attribute", table => table.HasComment(""));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(30);
            builder.Property(table => table.GroupId).HasColumnName("group_id");
            builder.Property(table => table.PropertyGroup).HasColumnName("property_group").HasMaxLength(20).HasDefaultValue(string.Empty).HasComment("静态属性的分组");
            builder.Property(table => table.Type).HasColumnName("type").HasMaxLength(1).HasDefaultValue(0);
            builder.Property(table => table.SearchType).HasColumnName("search_type").HasMaxLength(1).HasDefaultValue(0);
            builder.Property(table => table.InputType).HasColumnName("input_type").HasMaxLength(1).HasDefaultValue(0);
            builder.Property(table => table.DefaultValue).HasColumnName("default_value").HasDefaultValue(string.Empty);
            builder.Property(table => table.Position).HasColumnName("position").HasMaxLength(3).HasDefaultValue(99);
        }
    }
}
