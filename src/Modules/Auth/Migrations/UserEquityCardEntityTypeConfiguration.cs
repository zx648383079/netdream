using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Auth.Entities;

namespace NetDream.Modules.Auth.Migrations
{
    public class UserEquityCardEntityTypeConfiguration : IEntityTypeConfiguration<UserEquityCardEntity>
    {
        public void Configure(EntityTypeBuilder<UserEquityCardEntity> builder)
        {
            builder.ToTable("user_equity_card", table => table.HasComment("用户的权益卡"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.CardId).HasColumnName("card_id");
            builder.Property(table => table.Exp).HasColumnName("exp").HasDefaultValue(0);
            builder.Property(table => table.Status).HasColumnName("status").HasMaxLength(2).HasDefaultValue(0);
            builder.Property(table => table.ExpiredAt).HasColumnName("expired_at");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
