using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Forum.Entities;

namespace NetDream.Modules.Forum.Migrations
{
    public class ForumModeratorEntityTypeConfiguration : IEntityTypeConfiguration<ForumModeratorEntity>
    {
        public void Configure(EntityTypeBuilder<ForumModeratorEntity> builder)
        {
            builder.ToTable("bbs_forum_moderator", table => table.HasComment(""));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.ForumId).HasColumnName("forum_id").HasDefaultValue(0);
            builder.Property(table => table.RoleId).HasColumnName("role_id").HasDefaultValue(0);
        }
    }
}
