using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Auth.Entities;

namespace NetDream.Modules.Auth.Migrations
{
    public class BulletinUserEntityTypeConfiguration : IEntityTypeConfiguration<BulletinUserEntity>
    {
        public void Configure(EntityTypeBuilder<BulletinUserEntity> builder)
        {
            builder.ToTable("bulletin_user", table => table.HasComment(""));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.BulletinId).HasColumnName("bulletin_id");
            builder.Property(table => table.Status).HasColumnName("status").HasMaxLength(2).HasDefaultValue(0);
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
