using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Auth.Entities;
using NetDream.Modules.Auth.Repositories;

namespace NetDream.Modules.Auth.Migrations
{
    public class InviteCodeEntityTypeConfiguration : IEntityTypeConfiguration<InviteCodeEntity>
    {
        public void Configure(EntityTypeBuilder<InviteCodeEntity> builder)
        {
            builder.ToTable("user_invite_code", table => table.HasComment("ÑûÇëÂëÉú³É"));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Type).HasColumnName("type").HasMaxLength(1).HasDefaultValue(InviteRepository.TYPE_CODE);
            builder.Property(table => table.UserId).HasColumnName("user_id").HasDefaultValue(0);
            builder.Property(table => table.Amount).HasColumnName("amount").HasDefaultValue(1);
            builder.Property(table => table.InviteCount).HasColumnName("invite_count").HasDefaultValue(0);
            builder.Property(table => table.Token).HasColumnName("token").HasMaxLength(32);
            builder.Property(table => table.ExpiredAt).HasColumnName("expired_at");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
