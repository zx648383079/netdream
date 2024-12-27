using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Auth.Entities;

namespace NetDream.Modules.Auth.Migrations
{
    public class InviteLogEntityTypeConfiguration : IEntityTypeConfiguration<InviteLogEntity>
    {
        public void Configure(EntityTypeBuilder<InviteLogEntity> builder)
        {
            builder.ToTable("user_invite_log", table => table.HasComment("ÑûÇë¼ÇÂ¼"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.ParentId).HasColumnName("parent_id").HasDefaultValue(0);
            builder.Property(table => table.CodeId).HasColumnName("code_id").HasDefaultValue(0);
            builder.Property(table => table.Status).HasColumnName("status").HasMaxLength(1).HasDefaultValue(0);
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
