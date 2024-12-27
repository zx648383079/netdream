using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Auth.Entities;

namespace NetDream.Modules.Auth.Migrations
{
    public class RankEntityTypeConfiguration : IEntityTypeConfiguration<RankEntity>
    {
        public void Configure(EntityTypeBuilder<RankEntity> builder)
        {
            builder.ToTable("rank", table => table.HasComment(""));
            builder.HasKey(i => i.Id);
        }
    }
}
