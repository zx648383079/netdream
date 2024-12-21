using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Auth.Entities;

namespace NetDream.Modules.Auth.Migrations
{
    public class AdminLogEntityTypeConfiguration : IEntityTypeConfiguration<AdminLogEntity>
    {
        public void Configure(EntityTypeBuilder<AdminLogEntity> builder)
        {
            builder.ToTable("user_admin_log", table => table.HasComment("管理员操作记录"));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Ip).HasColumnName("ip").HasMaxLength(120);
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.ItemType).HasColumnName("item_type").HasMaxLength(2).HasDefaultValue(0);
            builder.Property(table => table.ItemId).HasColumnName("item_id").HasDefaultValue(0);
            builder.Property(table => table.Action).HasColumnName("action").HasMaxLength(30);
            builder.Property(table => table.Remark).HasColumnName("remark").HasDefaultValue(string.Empty);
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
