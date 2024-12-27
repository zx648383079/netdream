using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Contact.Entities;

namespace NetDream.Modules.Contact.Migrations
{
    public class FriendLinkEntityTypeConfiguration : IEntityTypeConfiguration<FriendLinkEntity>
    {
        public void Configure(EntityTypeBuilder<FriendLinkEntity> builder)
        {
            builder.ToTable("cif_friend_link", table => table.HasComment("ÓÑÇéÁ´½Ó"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(20);
            builder.Property(table => table.Url).HasColumnName("url").HasMaxLength(50);
            builder.Property(table => table.Logo).HasColumnName("logo").HasMaxLength(200).HasDefaultValue(string.Empty);
            builder.Property(table => table.Brief).HasColumnName("brief").HasDefaultValue(string.Empty);
            builder.Property(table => table.Email).HasColumnName("email").HasMaxLength(100).HasDefaultValue(string.Empty);
            builder.Property(table => table.Status).HasColumnName("status").HasDefaultValue(0);
            builder.Property(table => table.UserId).HasColumnName("user_id").HasDefaultValue(0);
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
