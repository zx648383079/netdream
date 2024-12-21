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
            builder.Property(table => table.Id).HasColumnName("id").HasComment("��˿ID");
            builder.Property(table => table.Openid).HasColumnName("openid").HasMaxLength(50).HasComment("΢��ID");
            builder.Property(table => table.Nickname).HasColumnName("nickname").HasMaxLength(20).HasDefaultValue(string.Empty).HasComment("�ǳ�");
            builder.Property(table => table.Sex).HasColumnName("sex").HasDefaultValue(0).HasComment("�Ա�");
            builder.Property(table => table.City).HasColumnName("city").HasMaxLength(40).HasDefaultValue(string.Empty).HasComment("���ڳ���");
            builder.Property(table => table.Country).HasColumnName("country").HasMaxLength(40).HasDefaultValue(string.Empty).HasComment("���ڹ���");
            builder.Property(table => table.Province).HasColumnName("province").HasMaxLength(40).HasDefaultValue(string.Empty).HasComment("����ʡ");
            builder.Property(table => table.Language).HasColumnName("language").HasMaxLength(40).HasDefaultValue(string.Empty).HasComment("�û�����");
            builder.Property(table => table.Avatar).HasColumnName("avatar").HasComment("�û�ͷ��");
            builder.Property(table => table.SubscribeAt).HasColumnName("subscribe_at");
            builder.Property(table => table.UnionId).HasColumnName("union_id").HasMaxLength(30).HasDefaultValue(string.Empty).HasComment("΢��ID");
            builder.Property(table => table.Remark).HasColumnName("remark").HasDefaultValue(string.Empty).HasComment("��ע");
            builder.Property(table => table.GroupId).HasColumnName("group_id").HasDefaultValue(0);
            builder.Property(table => table.BotId).HasColumnName("bot_id").HasComment("����΢�Ź��ں�ID");
            builder.Property(table => table.NoteName).HasColumnName("note_name").HasMaxLength(20).HasDefaultValue(string.Empty).HasComment("��ע��");
            builder.Property(table => table.Status).HasColumnName("status").HasDefaultValue(UserModel.STATUS_SUBSCRIBED)
                .HasComment("��ע״̬");
            builder.Property(table => table.IsBlack).HasColumnName("is_black").HasDefaultValue(0).HasComment("�Ƿ��Ǻ�����");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
