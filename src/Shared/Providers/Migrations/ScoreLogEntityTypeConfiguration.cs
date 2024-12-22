using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Shared.Providers.Entities;

namespace NetDream.Shared.Providers.Migrations
{
    public class ScoreLogEntityTypeConfiguration(string prefix) : IEntityTypeConfiguration<ScoreLogEntity>
    {
        public void Configure(EntityTypeBuilder<ScoreLogEntity> builder)
        {
            builder.ToTable(prefix + "_score", table => table.HasComment("ÆÀ·Ö¼ÇÂ¼"));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.ItemType).HasColumnName("item_type").HasMaxLength(1).HasDefaultValue(0);
            builder.Property(table => table.ItemId).HasColumnName("item_id");
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.Score).HasColumnName("score").HasDefaultValue(6);
            builder.Property(table => table.FromType).HasColumnName("from_type").HasMaxLength(1).HasDefaultValue(0);
            builder.Property(table => table.FromId).HasColumnName("from_id"); 
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
