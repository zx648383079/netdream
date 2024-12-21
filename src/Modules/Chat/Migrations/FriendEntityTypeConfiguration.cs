using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Chat.Entities;

namespace NetDream.Modules.Chat.Migrations
{
    public class FriendEntityTypeConfiguration : IEntityTypeConfiguration<FriendEntity>
    {
        public void Configure(EntityTypeBuilder<FriendEntity> builder)
        {
            builder.ToTable("chat_friend", table => table.HasComment(""));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(50)
                .HasDefaultValue(string.Empty).HasComment("备注");
            builder.Property(table => table.ClassifyId).HasColumnName("classify_id").HasDefaultValue(1).HasComment("分组/1为默认分组，0为黑名单");
            builder.Property(table => table.UserId).HasColumnName("user_id").HasComment("用户");
            builder.Property(table => table.BelongId).HasColumnName("belong_id").HasComment("归属");
            builder.Property(table => table.Status).HasColumnName("status").HasDefaultValue(0).HasComment("是否互相关注");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
