using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.UserIdentity.Entities;

namespace NetDream.Modules.UserIdentity.Migrations
{
    public class UserRoleEntityTypeConfiguration : IEntityTypeConfiguration<UserRoleEntity>
    {
        public void Configure(EntityTypeBuilder<UserRoleEntity> builder)
        {
            builder.ToTable("rbac_user_role", table => table.HasComment(""));
            builder.HasNoKey();
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.RoleId).HasColumnName("role_id");
        }
    }
}
