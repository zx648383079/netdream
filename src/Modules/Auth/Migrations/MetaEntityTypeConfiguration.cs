using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Auth.Entities;

namespace NetDream.Modules.Auth.Migrations
{
    public class MetaEntityTypeConfiguration : IEntityTypeConfiguration<UserMetaEntity>
    {
        public void Configure(EntityTypeBuilder<UserMetaEntity> builder)
        {
            builder.ToTable("user_meta");
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(100);
            builder.Property(table => table.Content).HasColumnName("content");
        }
    }
}
