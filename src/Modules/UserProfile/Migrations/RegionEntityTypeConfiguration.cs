using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.UserProfile.Entities;

namespace NetDream.Modules.UserProfile.Migrations
{
    public class RegionEntityTypeConfiguration : IEntityTypeConfiguration<RegionEntity>
    {
        public void Configure(EntityTypeBuilder<RegionEntity> builder)
        {
            builder.ToTable("base_region", table => table.HasComment("ÇøÓòµØÍ¼"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(30);
            builder.Property(table => table.ParentId).HasColumnName("parent_id").HasDefaultValue(0);
            builder.Property(table => table.FullName).HasColumnName("full_name").HasMaxLength(100).HasDefaultValue(string.Empty);
        }
    }
}
