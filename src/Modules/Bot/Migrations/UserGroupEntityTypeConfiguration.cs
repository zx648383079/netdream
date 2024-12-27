using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Bot.Entities;

namespace NetDream.Modules.Bot.Migrations
{
    public class UserGroupEntityTypeConfiguration : IEntityTypeConfiguration<UserGroupEntity>
    {
        public void Configure(EntityTypeBuilder<UserGroupEntity> builder)
        {
            builder.ToTable("bot_user_group", table => table.HasComment(""));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id").HasComment("����");
            builder.Property(table => table.BotId).HasColumnName("bot_id").HasComment("����΢�Ź��ں�ID");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(20).HasComment("����");
            builder.Property(table => table.TagId).HasColumnName("tag_id").HasMaxLength(20).HasDefaultValue(string.Empty).HasComment("����ƽ̨��ǩid");
        }
    }
}
