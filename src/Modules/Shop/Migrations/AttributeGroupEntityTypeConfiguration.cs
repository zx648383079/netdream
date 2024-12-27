using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Migrations
{
    public class AttributeGroupEntityTypeConfiguration : IEntityTypeConfiguration<AttributeGroupEntity>
    {
        public void Configure(EntityTypeBuilder<AttributeGroupEntity> builder)
        {
            builder.ToTable("shop_attribute_group", table => table.HasComment(""));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(30);
            builder.Property(table => table.PropertyGroups).HasColumnName("property_groups").HasDefaultValue(string.Empty).HasComment("静态属性的可选分组，以换行符分隔");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
