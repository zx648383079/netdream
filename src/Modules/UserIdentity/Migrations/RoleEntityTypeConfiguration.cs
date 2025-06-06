using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.UserIdentity.Entities;

namespace NetDream.Modules.UserIdentity.Migrations
{
    public class RoleEntityTypeConfiguration : IEntityTypeConfiguration<RoleEntity>
    {
        public void Configure(EntityTypeBuilder<RoleEntity> builder)
        {
            builder.ToTable("rbac_role", table => table.HasComment(""));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.HasIndex(table => table.Name).IsUnique();
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(40);
            builder.Property(table => table.DisplayName).HasColumnName("display_name").HasMaxLength(100).HasDefaultValue(string.Empty);
            builder.Property(table => table.Description).HasColumnName("description").HasDefaultValue(string.Empty);
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
