using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Migrations
{
    public class NavigationEntityTypeConfiguration : IEntityTypeConfiguration<NavigationEntity>
    {
        public void Configure(EntityTypeBuilder<NavigationEntity> builder)
        {
            builder.ToTable("Navigation", table => table.HasComment(""));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Type).HasColumnName("type").HasMaxLength(10).HasDefaultValue("middle");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(100);
            builder.Property(table => table.Url).HasColumnName("url").HasMaxLength(200);
            builder.Property(table => table.Target).HasColumnName("target").HasMaxLength(10);
            builder.Property(table => table.Visible).HasColumnName("visible").HasDefaultValue(1);
            builder.Property(table => table.Position).HasColumnName("position").HasMaxLength(2).HasDefaultValue(99);
        }
    }
}
