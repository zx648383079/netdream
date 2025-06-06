using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.UserIdentity.Entities;

namespace NetDream.Modules.UserIdentity.Migrations
{
    public class RolePermissionEntityTypeConfiguration : IEntityTypeConfiguration<RolePermissionEntity>
    {
        public void Configure(EntityTypeBuilder<RolePermissionEntity> builder)
        {
            builder.ToTable("rbac_role_permission", table => table.HasComment(""));
            builder.HasNoKey();
            builder.Property(table => table.RoleId).HasColumnName("role_id");
            builder.Property(table => table.PermissionId).HasColumnName("permission_id");
        }
    }
}
