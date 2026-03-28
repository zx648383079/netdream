using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Clan.Entities;

namespace NetDream.Modules.Clan.Migrations
{
    public class FamilySpouseEntityTypeConfiguration : IEntityTypeConfiguration<FamilySpouseEntity>
    {
        public void Configure(EntityTypeBuilder<FamilySpouseEntity> builder)
        {
            builder.ToTable("clan_family_spouse", table => table.HasComment("家族婚姻表"));
            builder.HasKey(table => table.Id);
            builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
            
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at").HasMaxLength(10).HasDefaultValue(0);
            builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasMaxLength(10).HasDefaultValue(0);

        }
    }
}
