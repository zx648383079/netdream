using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.UserAccount.Entities;

namespace NetDream.Modules.UserAccount.Migrations
{
    public class ApplyLogEntityTypeConfiguration : IEntityTypeConfiguration<ApplyLogEntity>
    {
        public void Configure(EntityTypeBuilder<ApplyLogEntity> builder)
        {
            builder.ToTable("user_apply_log", table => table.HasComment("用户申请记录"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.Type).HasColumnName("type").HasMaxLength(1).HasDefaultValue(0);
            builder.Property(table => table.ItemType).HasColumnName("item_type").HasMaxLength(1).HasDefaultValue(0).HasComment("向谁发起请求");
            builder.Property(table => table.ItemId).HasColumnName("item_id").HasDefaultValue(0);
            builder.Property(table => table.Money).HasColumnName("money").HasDefaultValue(0);
            builder.Property(table => table.Remark).HasColumnName("remark").HasDefaultValue(string.Empty);
            builder.Property(table => table.Status).HasColumnName("status").HasMaxLength(2).HasDefaultValue(0);
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
