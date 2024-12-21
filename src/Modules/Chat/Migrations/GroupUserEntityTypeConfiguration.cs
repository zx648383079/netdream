using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Chat.Entities;

namespace NetDream.Modules.Chat.Migrations
{
    public class GroupUserEntityTypeConfiguration : IEntityTypeConfiguration<GroupUserEntity>
    {
        public void Configure(EntityTypeBuilder<GroupUserEntity> builder)
        {
            builder.ToTable("chat_group_user", table => table.HasComment(""));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.GroupId).HasColumnName("group_id").HasComment("群");
            builder.Property(table => table.UserId).HasColumnName("user_id").HasComment("用户");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(50)
                .HasDefaultValue(string.Empty).HasComment("群备注");
            builder.Property(table => table.RoleId).HasColumnName("role_id").HasDefaultValue(0).HasComment("管理员等级");
            builder.Property(table => table.Status).HasColumnName("status").HasMaxLength(1)
                .HasDefaultValue(5).HasComment("用户状态/禁言或");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
