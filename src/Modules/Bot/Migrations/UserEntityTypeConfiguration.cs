using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Bot.Entities;

namespace NetDream.Modules.Bot.Migrations
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.ToTable("bot_user", table => table.HasComment(""));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id").HasComment("粉丝ID");
            builder.Property(table => table.Openid).HasColumnName("openid").HasMaxLength(50).HasComment("微信ID");
            builder.Property(table => table.Nickname).HasColumnName("nickname").HasMaxLength(20).HasDefaultValue(string.Empty).HasComment("昵称");
            builder.Property(table => table.Sex).HasColumnName("sex").HasDefaultValue(0).HasComment("性别");
            builder.Property(table => table.City).HasColumnName("city").HasMaxLength(40).HasDefaultValue(string.Empty).HasComment("所在城市");
            builder.Property(table => table.Country).HasColumnName("country").HasMaxLength(40).HasDefaultValue(string.Empty).HasComment("所在国家");
            builder.Property(table => table.Province).HasColumnName("province").HasMaxLength(40).HasDefaultValue(string.Empty).HasComment("所在省");
            builder.Property(table => table.Language).HasColumnName("language").HasMaxLength(40).HasDefaultValue(string.Empty).HasComment("用户语言");
            builder.Property(table => table.Avatar).HasColumnName("avatar").HasComment("用户头像");
            builder.Property(table => table.SubscribeAt).HasColumnName("subscribe_at");
            builder.Property(table => table.UnionId).HasColumnName("union_id").HasMaxLength(30).HasDefaultValue(string.Empty).HasComment("微信ID");
            builder.Property(table => table.Remark).HasColumnName("remark").HasDefaultValue(string.Empty).HasComment("备注");
            builder.Property(table => table.GroupId).HasColumnName("group_id").HasDefaultValue(0);
            builder.Property(table => table.BotId).HasColumnName("bot_id").HasComment("所属微信公众号ID");
            builder.Property(table => table.NoteName).HasColumnName("note_name").HasMaxLength(20).HasDefaultValue(string.Empty).HasComment("备注名");
            builder.Property(table => table.Status).HasColumnName("status").HasDefaultValue(UserModel.STATUS_SUBSCRIBED)
                .HasComment("关注状态");
            builder.Property(table => table.IsBlack).HasColumnName("is_black").HasDefaultValue(0).HasComment("是否是黑名单");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
