using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.UserIdentity.Entities;

namespace NetDream.Modules.UserIdentity.Migrations
{
    public class EquityCardEntityTypeConfiguration : IEntityTypeConfiguration<EquityCardEntity>
    {
        public void Configure(EntityTypeBuilder<EquityCardEntity> builder)
        {
            builder.ToTable("equity_card", table => table.HasComment("有期限的权益卡"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(32);
            builder.Property(table => table.Icon).HasColumnName("icon");
            builder.Property(table => table.Configure).HasColumnName("configure").HasMaxLength(200).HasDefaultValue(string.Empty);
            builder.Property(table => table.Status).HasColumnName("status").HasMaxLength(2).HasDefaultValue(0);
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");

            builder.HasMany(p => p.Items)
              .WithOne(b => b.Card)
              .HasPrincipalKey(p => p.Id)
              .HasForeignKey(b => b.CardId);
        }
    }
}
