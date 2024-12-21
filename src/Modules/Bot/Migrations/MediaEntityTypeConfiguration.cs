using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Bot.Entities;

namespace NetDream.Modules.Bot.Migrations
{
    public class MediaEntityTypeConfiguration : IEntityTypeConfiguration<MediaEntity>
    {
        public void Configure(EntityTypeBuilder<MediaEntity> builder)
        {
            builder.ToTable("bot_media", table => table.HasComment(""));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.BotId).HasColumnName("bot_id").HasComment("����΢�Ź��ں�ID");
            builder.Property(table => table.Type).HasColumnName("type").HasMaxLength(10).HasComment("�ز�����");
            builder.Property(table => table.MaterialType).HasColumnName("material_type").HasDefaultValue(MediaModel.MATERIAL_PERMANENT)
                .HasComment("�ز����:����/��ʱ");
            builder.Property(table => table.Title).HasColumnName("title").HasMaxLength(200).HasComment("�زı���");
            builder.Property(table => table.Thumb).HasColumnName("thumb").HasMaxLength(200).HasDefaultValue(string.Empty).HasComment("ͼ�ĵķ���");
            builder.Property(table => table.ShowCover).HasColumnName("show_cover").HasDefaultValue(0).HasComment("��ʾͼ�ĵķ���");
            builder.Property(table => table.OpenComment).HasColumnName("open_comment").HasDefaultValue(0).HasComment("ͼ���Ƿ��������");
            builder.Property(table => table.OnlyComment).HasColumnName("only_comment").HasDefaultValue(0).HasComment("ͼ�Ŀ������۵���");
            builder.Property(table => table.Content).HasColumnName("content").HasComment("�ز�����");
            builder.Property(table => table.ParentId).HasColumnName("parent_id").HasDefaultValue(0).HasComment("ͼ�ĸ�id");
            builder.Property(table => table.MediaId).HasColumnName("media_id").HasMaxLength(100).HasDefaultValue(string.Empty).HasComment("�ز�ID");
            builder.Property(table => table.Url).HasColumnName("url").HasDefaultValue(string.Empty).HasComment("ͼƬ��url");
            builder.Property(table => table.PublishStatus).HasColumnName("publish_status").HasMaxLength(1).HasDefaultValue(0).HasComment("��ǰ������״̬");
            builder.Property(table => table.PublishId).HasColumnName("publish_id").HasMaxLength(50).HasDefaultValue(string.Empty).HasComment("��ǰ������id, publish_status=6,Ϊ�ݸ�=7,Ϊ������publish_id��Ϊ�ݸ�=8,Ϊ�����ɹ���article_id");
            builder.Property(table => table.ExpiredAt).HasColumnName("expired_at").HasComment("��ʱ�زĵĹ���ʱ��");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
