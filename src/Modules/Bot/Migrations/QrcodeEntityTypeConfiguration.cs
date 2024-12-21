using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Bot.Entities;

namespace NetDream.Modules.Bot.Migrations
{
    public class QrcodeEntityTypeConfiguration : IEntityTypeConfiguration<QrcodeEntity>
    {
        public void Configure(EntityTypeBuilder<QrcodeEntity> builder)
        {
            builder.ToTable("bot_qrcode", table => table.HasComment("΢�Ŷ�ά��"));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.BotId).HasColumnName("bot_id").HasComment("����΢�Ź��ں�ID");
            builder.Property(table => table.Name).HasColumnName("name").HasComment("ʹ����;");
            builder.Property(table => table.Type).HasColumnName("type").HasDefaultValue(0).HasComment("���û���ʱ");
            builder.Property(table => table.SceneType).HasColumnName("scene_type").HasDefaultValue(0).HasComment("���ֻ��ַ���");
            builder.Property(table => table.SceneStr).HasColumnName("scene_str").HasDefaultValue(string.Empty).HasComment("����ֵ");
            builder.Property(table => table.SceneId).HasColumnName("scene_id").HasDefaultValue(0).HasComment("����ֵID����ʱ��ά��ʱΪ32λ��0���ͣ����ö�ά��ʱ���ֵΪ100000");
            builder.Property(table => table.ExpireTime).HasColumnName("expire_time").HasDefaultValue(0).HasComment("�����¼�/s");
            builder.Property(table => table.QrUrl).HasColumnName("qr_url").HasDefaultValue(string.Empty).HasComment("��ά���ַ");
            builder.Property(table => table.Url).HasColumnName("url").HasDefaultValue(string.Empty).HasComment("������ĵ�ַ");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
