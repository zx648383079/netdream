using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Chat.Entities;

namespace NetDream.Modules.Chat.Migrations
{
    public class FriendClassifyEntityTypeConfiguration : IEntityTypeConfiguration<FriendClassifyEntity>
    {
        public void Configure(EntityTypeBuilder<FriendClassifyEntity> builder)
        {
            builder.ToTable("chat_friend_classify", table => table.HasComment(""));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(100).HasComment("分组名");
            builder.Property(table => table.UserId).HasColumnName("user_id").HasComment("用户");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
