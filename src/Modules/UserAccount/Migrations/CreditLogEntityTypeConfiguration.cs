using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.UserAccount.Entities;

namespace NetDream.Modules.UserAccount.Migrations
{
    public class CreditLogEntityTypeConfiguration : IEntityTypeConfiguration<CreditLogEntity>
    {
        public void Configure(EntityTypeBuilder<CreditLogEntity> builder)
        {
            builder.ToTable("user_credit_log", table => table.HasComment("账户积分变动表"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.UserId).HasColumnName("user_id").HasDefaultValue(0);
            builder.Property(table => table.Type).HasColumnName("type").HasMaxLength(1).HasDefaultValue(99);
            builder.Property(table => table.ItemId).HasColumnName("item_id").HasDefaultValue(0);
            builder.Property(table => table.Credits).HasColumnName("credits").HasComment("本次发生积分");
            builder.Property(table => table.TotalCredits).HasColumnName("total_credits").HasComment("当前账户积分");
            builder.Property(table => table.Status).HasColumnName("status").HasMaxLength(2).HasDefaultValue(0);
            builder.Property(table => table.Remark).HasColumnName("remark").HasDefaultValue(string.Empty);
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
