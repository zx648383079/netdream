using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Shop.Entities;
using NetDream.Modules.Shop.Models;

namespace NetDream.Modules.Shop.Migrations
{
    public class ActivityEntityTypeConfiguration : IEntityTypeConfiguration<ActivityEntity>
    {
        public void Configure(EntityTypeBuilder<ActivityEntity> builder)
        {
            builder.ToTable("Activity", table => table.HasComment(""));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(40);
            builder.Property(table => table.Thumb).HasColumnName("thumb").HasMaxLength(200).HasDefaultValue(string.Empty);
            builder.Property(table => table.Description).HasColumnName("description").HasDefaultValue(string.Empty);
            builder.Property(table => table.Type).HasColumnName("type").HasMaxLength(2).HasDefaultValue(ActivityType.TYPE_AUCTION);
            builder.Property(table => table.ScopeType).HasColumnName("scope_type").HasMaxLength(1).HasDefaultValue(0).HasComment("商品范围类型");
            builder.Property(table => table.Scope).HasColumnName("scope").HasComment("商品范围值");
            builder.Property(table => table.Configure).HasColumnName("configure").HasComment("其他配置信息");
            builder.Property(table => table.Status).HasColumnName("status").HasDefaultValue(ActivityType.STATUS_NONE).HasComment("开启关闭");
            builder.Property(table => table.StartAt).HasColumnName("start_at");
            builder.Property(table => table.EndAt).HasColumnName("end_at");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
