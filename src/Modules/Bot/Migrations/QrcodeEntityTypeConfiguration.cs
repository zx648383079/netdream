using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Bot.Entities;

namespace NetDream.Modules.Bot.Migrations
{
    public class QrcodeEntityTypeConfiguration : IEntityTypeConfiguration<QrcodeEntity>
    {
        public void Configure(EntityTypeBuilder<QrcodeEntity> builder)
        {
            builder.ToTable("bot_qrcode", table => table.HasComment("微信二维码"));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.BotId).HasColumnName("bot_id").HasComment("所属微信公众号ID");
            builder.Property(table => table.Name).HasColumnName("name").HasComment("使用用途");
            builder.Property(table => table.Type).HasColumnName("type").HasDefaultValue(0).HasComment("永久或临时");
            builder.Property(table => table.SceneType).HasColumnName("scene_type").HasDefaultValue(0).HasComment("数字或字符串");
            builder.Property(table => table.SceneStr).HasColumnName("scene_str").HasDefaultValue(string.Empty).HasComment("场景值");
            builder.Property(table => table.SceneId).HasColumnName("scene_id").HasDefaultValue(0).HasComment("场景值ID，临时二维码时为32位非0整型，永久二维码时最大值为100000");
            builder.Property(table => table.ExpireTime).HasColumnName("expire_time").HasDefaultValue(0).HasComment("过期事件/s");
            builder.Property(table => table.QrUrl).HasColumnName("qr_url").HasDefaultValue(string.Empty).HasComment("二维码地址");
            builder.Property(table => table.Url).HasColumnName("url").HasDefaultValue(string.Empty).HasComment("解析后的地址");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
