using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Migrations
{
    public class AdPositionEntityTypeConfiguration : IEntityTypeConfiguration<AdPositionEntity>
    {
        public void Configure(EntityTypeBuilder<AdPositionEntity> builder)
        {
            builder.ToTable("AdPosition", table => table.HasComment(""));
            builder.HasKey("id");
        }
    }
}
