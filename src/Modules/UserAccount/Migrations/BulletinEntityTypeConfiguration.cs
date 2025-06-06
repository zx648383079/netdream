using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.UserAccount.Entities;

namespace NetDream.Modules.UserAccount.Migrations
{
    public class BulletinEntityTypeConfiguration : IEntityTypeConfiguration<BulletinEntity>
    {
        public void Configure(EntityTypeBuilder<BulletinEntity> builder)
        {
            builder.ToTable("bulletin", table => table.HasComment(""));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Title).HasColumnName("title").HasMaxLength(100);
            builder.Property(table => table.Content).HasColumnName("content");
            builder.Property(table => table.ExtraRule).HasColumnName("extra_rule").HasDefaultValue(string.Empty);
            builder.Property(table => table.Type).HasColumnName("type").HasMaxLength(2).HasDefaultValue(0);
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");

            builder.HasMany(p => p.Items)
                .WithOne(b => b.Bulletin)
                .HasPrincipalKey(p => p.Id)
                .HasForeignKey(b => b.BulletinId);
        }
    }
}
