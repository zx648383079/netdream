using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Bot.Entities;

namespace NetDream.Modules.Bot.Migrations
{
    public class ReplyEntityTypeConfiguration : IEntityTypeConfiguration<ReplyEntity>
    {
        public void Configure(EntityTypeBuilder<ReplyEntity> builder)
        {
            builder.ToTable("bot_reply", table => table.HasComment("΢�Żظ�"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.BotId).HasColumnName("bot_id").HasComment("����΢�Ź��ں�ID");
            builder.Property(table => table.Event).HasColumnName("event").HasMaxLength(20).HasComment("�¼�");
            builder.Property(table => table.Keywords).HasColumnName("keywords").HasMaxLength(60).HasDefaultValue(string.Empty).HasComment("�ؼ���");
            builder.Property(table => table.Match).HasColumnName("match").HasDefaultValue(0).HasComment("�ؼ���ƥ��ģʽ");
            builder.Property(table => table.Content).HasColumnName("content").HasComment("΢�ŷ�������");
            builder.Property(table => table.Type).HasColumnName("type").HasMaxLength(1).HasDefaultValue(0).HasComment("�ز�����");
            builder.Property(table => table.Status).HasColumnName("status").HasDefaultValue(1).HasComment("����");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
