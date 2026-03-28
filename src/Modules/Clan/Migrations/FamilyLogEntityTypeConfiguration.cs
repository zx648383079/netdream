using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Clan.Entities;

namespace NetDream.Modules.Clan.Migrations
{
    public class FamilyLogEntityTypeConfiguration : IEntityTypeConfiguration<FamilyLogEntity>
    {
        public void Configure(EntityTypeBuilder<FamilyLogEntity> builder)
        {
            builder.ToTable("clan_family_log", table => table.HasComment("家族历史记录表"));
            builder.HasKey(table => table.Id);
            builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
            
            builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasMaxLength(10).HasDefaultValue(0);

        }
    }
}
