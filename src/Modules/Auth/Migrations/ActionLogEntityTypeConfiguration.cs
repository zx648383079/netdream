using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Auth.Entities;

namespace NetDream.Modules.Auth.Migrations
{
    public class ActionLogEntityTypeConfiguration : IEntityTypeConfiguration<ActionLogEntity>
    {
        public void Configure(EntityTypeBuilder<ActionLogEntity> builder)
        {
            builder.ToTable("user_action_log", table => table.HasComment("²Ù×÷¼ÇÂ¼"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Ip).HasColumnName("ip").HasMaxLength(120);
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.Action).HasColumnName("action").HasMaxLength(30);
            builder.Property(table => table.Remark).HasColumnName("remark").HasDefaultValue(string.Empty);
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
