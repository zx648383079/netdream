using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Bot.Entities;

namespace NetDream.Modules.Bot.Migrations
{
    public class MenuEntityTypeConfiguration : IEntityTypeConfiguration<MenuEntity>
    {
        public void Configure(EntityTypeBuilder<MenuEntity> builder)
        {
            builder.ToTable("bot_menu", table => table.HasComment("΢�Ų˵�"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.BotId).HasColumnName("bot_id").HasComment("����΢�Ź��ں�ID");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(100).HasComment("�˵�����");
            builder.Property(table => table.Type).HasColumnName("type").HasMaxLength(1).HasDefaultValue(0).HasComment("�˵�����");
            builder.Property(table => table.Content).HasColumnName("content").HasMaxLength(500).HasDefaultValue(string.Empty).HasComment("�˵�����");
            builder.Property(table => table.ParentId).HasColumnName("parent_id").HasDefaultValue(0);
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
