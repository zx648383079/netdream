using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Bot.Entities;
using NetDream.Modules.Bot.Repositories;

namespace NetDream.Modules.Bot.Migrations
{
    public class BotEntityTypeConfiguration : IEntityTypeConfiguration<BotEntity>
    {
        public void Configure(EntityTypeBuilder<BotEntity> builder)
        {
            builder.ToTable("bot", table => table.HasComment(""));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.PlatformType).HasColumnName("platform_type").HasMaxLength(1)
                .HasDefaultValue(BotRepository.PLATFORM_TYPE_WX).HasComment("���ں�ƽ̨����");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(40).HasComment("���ں�����");
            builder.Property(table => table.Token).HasColumnName("token").HasMaxLength(32).HasComment("΢�ŷ��������֤token");
            builder.Property(table => table.AccessToken).HasColumnName("access_token").HasDefaultValue(string.Empty).HasComment("����΢�ŷ�����֤token");
            builder.Property(table => table.Account).HasColumnName("account").HasMaxLength(30).HasDefaultValue(string.Empty).HasComment("΢�ź�");
            builder.Property(table => table.Original).HasColumnName("original").HasMaxLength(40).HasDefaultValue(string.Empty).HasComment("ԭʼID");
            builder.Property(table => table.Type).HasColumnName("type").HasMaxLength(1).HasDefaultValue(0).HasComment("���ں�����");
            builder.Property(table => table.Appid).HasColumnName("appid").HasMaxLength(50).HasDefaultValue(string.Empty).HasComment("���ںŵ�AppID");
            builder.Property(table => table.Secret).HasColumnName("secret").HasMaxLength(50).HasDefaultValue(string.Empty).HasComment("���ںŵ�AppSecret");
            builder.Property(table => table.AesKey).HasColumnName("aes_key").HasMaxLength(43).HasDefaultValue(string.Empty).HasComment("��Ϣ������ԿEncodingAesKey");
            builder.Property(table => table.Avatar).HasColumnName("avatar").HasDefaultValue(string.Empty).HasComment("ͷ���ַ");
            builder.Property(table => table.Qrcode).HasColumnName("qrcode").HasDefaultValue(string.Empty).HasComment("��ά���ַ");
            builder.Property(table => table.Address).HasColumnName("address").HasDefaultValue(string.Empty).HasComment("���ڵ�ַ");
            builder.Property(table => table.Description).HasColumnName("description").HasDefaultValue(string.Empty).HasComment("���ںż��");
            builder.Property(table => table.Username).HasColumnName("username").HasMaxLength(40).HasDefaultValue(string.Empty).HasComment("΢�Ź�����¼��");
            builder.Property(table => table.Password).HasColumnName("password").HasMaxLength(32).HasDefaultValue(string.Empty).HasComment("΢�Ź�����¼����");
            builder.Property(table => table.Status).HasColumnName("status").HasDefaultValue(BotRepository.STATUS_INACTIVE).HasComment("״̬");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
