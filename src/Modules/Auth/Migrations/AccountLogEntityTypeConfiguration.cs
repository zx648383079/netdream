using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Auth.Entities;

namespace NetDream.Modules.Auth.Migrations
{
    public class AccountLogEntityTypeConfiguration : IEntityTypeConfiguration<AccountLogEntity>
    {
        public void Configure(EntityTypeBuilder<AccountLogEntity> builder)
        {
            builder.ToTable("user_account_log", table => table.HasComment("账户资金变动表"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.UserId).HasColumnName("user_id").HasDefaultValue(0);
            builder.Property(table => table.Type).HasColumnName("type").HasMaxLength(1).HasDefaultValue(99);
            builder.Property(table => table.ItemId).HasColumnName("item_id").HasDefaultValue(0);
            builder.Property(table => table.Money).HasColumnName("money").HasComment("本次发生金额");
            builder.Property(table => table.TotalMoney).HasColumnName("total_money").HasComment("当前账户余额");
            builder.Property(table => table.Status).HasColumnName("status").HasMaxLength(2).HasDefaultValue(0);
            builder.Property(table => table.Remark).HasColumnName("remark").HasDefaultValue(string.Empty);
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
