using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Shop.Entities;
using NetDream.Modules.Shop.Repositories;

namespace NetDream.Modules.Shop.Migrations
{
    public class AuctionLogEntityTypeConfiguration : IEntityTypeConfiguration<AuctionLogEntity>
    {
        public void Configure(EntityTypeBuilder<AuctionLogEntity> builder)
        {
            builder.ToTable("AuctionLog", table => table.HasComment(""));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.ActId).HasColumnName("act_id");
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.Bid).HasColumnName("bid").HasMaxLength(8).HasDefaultValue(0)
                .HasComment("出价");
            builder.Property(table => table.Amount).HasColumnName("amount").HasDefaultValue(1).HasComment("出价数量");
            builder.Property(table => table.Status).HasColumnName("status").HasMaxLength(1).HasDefaultValue(ActivityRepository.STATUS_NONE);
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
