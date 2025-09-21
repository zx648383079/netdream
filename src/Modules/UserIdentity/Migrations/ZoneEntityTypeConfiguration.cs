using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.UserIdentity.Entities;

namespace NetDream.Modules.UserIdentity.Migrations
{
    public class ZoneEntityTypeConfiguration : IEntityTypeConfiguration<ZoneEntity>
    {
        public void Configure(EntityTypeBuilder<ZoneEntity> builder)
        {
            builder.ToTable("user_zone", table => table.HasComment("分区"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(32);
            builder.Property(table => table.Icon).HasColumnName("icon").HasMaxLength(255);
            builder.Property(table => table.Description).HasColumnName("description").HasMaxLength(255)
                .HasDefaultValue(string.Empty);
            builder.Property(table => table.IsOpen).HasColumnName("is_open").HasMaxLength(1).HasDefaultValue(0);
            builder.Property(table => table.Status).HasColumnName("status").HasMaxLength(1).HasDefaultValue(0);
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
