using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Migrations
{
    public class BankCardEntityTypeConfiguration : IEntityTypeConfiguration<BankCardEntity>
    {
        public void Configure(EntityTypeBuilder<BankCardEntity> builder)
        {
            builder.ToTable("BankCard", table => table.HasComment("用户银行卡表"));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.Bank).HasColumnName("bank").HasMaxLength(50).HasComment("银行名");
            builder.Property(table => table.Type).HasColumnName("type").HasMaxLength(1).HasDefaultValue(0).HasComment("卡类型: 0 储蓄卡 1 信用卡");
            builder.Property(table => table.CardNo).HasColumnName("card_no").HasMaxLength(30).HasComment("卡号码");
            builder.Property(table => table.ExpiryDate).HasColumnName("expiry_date").HasMaxLength(30).HasDefaultValue(string.Empty).HasComment("卡有效期");
            builder.Property(table => table.Status).HasColumnName("status").HasMaxLength(2).HasDefaultValue(0).HasComment("审核状态");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
