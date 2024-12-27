using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Chat.Entities;

namespace NetDream.Modules.Chat.Migrations
{
    public class GroupEntityTypeConfiguration : IEntityTypeConfiguration<GroupEntity>
    {
        public void Configure(EntityTypeBuilder<GroupEntity> builder)
        {
            builder.ToTable("chat_group", table => table.HasComment(""));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(50).HasComment("群名");
            builder.Property(table => table.Logo).HasColumnName("logo").HasMaxLength(100).HasComment("群LOGO");
            builder.Property(table => table.Description).HasColumnName("description").HasDefaultValue(string.Empty)
                .HasComment("群说明");
            builder.Property(table => table.UserId).HasColumnName("user_id").HasComment("用户");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
