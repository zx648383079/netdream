using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Migrations
{
    public class LotteryLogEntityTypeConfiguration : IEntityTypeConfiguration<LotteryLogEntity>
    {
        public void Configure(EntityTypeBuilder<LotteryLogEntity> builder)
        {
            builder.ToTable("LotteryLog", table => table.HasComment("ÓÃ»§³é½±¼ÇÂ¼"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.ActId).HasColumnName("act_id");
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.ItemType).HasColumnName("item_type").HasMaxLength(1).HasDefaultValue(0);
            builder.Property(table => table.ItemId).HasColumnName("item_id").HasDefaultValue(0);
            builder.Property(table => table.Amount).HasColumnName("amount").HasDefaultValue(0);
            builder.Property(table => table.Status).HasColumnName("status").HasMaxLength(1).HasDefaultValue(0);
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
