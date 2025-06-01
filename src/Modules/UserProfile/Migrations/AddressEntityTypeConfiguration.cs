using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.UserProfile.Entities;

namespace NetDream.Modules.UserProfile.Migrations
{
    public class AddressEntityTypeConfiguration : IEntityTypeConfiguration<AddressEntity>
    {
        public void Configure(EntityTypeBuilder<AddressEntity> builder)
        {
            builder.ToTable("user_address", table => table.HasComment(""));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(30);
            builder.Property(table => table.RegionId).HasColumnName("region_id");
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.Tel).HasColumnName("tel").HasMaxLength(11);
            builder.Property(table => table.Address).HasColumnName("address");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
