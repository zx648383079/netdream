using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Clan.Entities;

namespace NetDream.Modules.Clan.Migrations
{
    public class ClanEntityTypeConfiguration : IEntityTypeConfiguration<ClanEntity>
    {
        public void Configure(EntityTypeBuilder<ClanEntity> builder)
        {
            builder.ToTable("clan", table => table.HasComment("家族表"));
            builder.HasKey(table => table.Id);
            builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
            builder.Property(table => table.UserId).HasColumnName("user_id").HasMaxLength(10);
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(200);
            
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at").HasMaxLength(10).HasDefaultValue(0);
            builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasMaxLength(10).HasDefaultValue(0);

        }
    }
}
