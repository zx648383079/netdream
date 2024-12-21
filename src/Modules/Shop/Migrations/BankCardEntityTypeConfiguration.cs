using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Migrations
{
    public class BankCardEntityTypeConfiguration : IEntityTypeConfiguration<BankCardEntity>
    {
        public void Configure(EntityTypeBuilder<BankCardEntity> builder)
        {
            builder.ToTable("BankCard", table => table.HasComment("�û����п���"));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.Bank).HasColumnName("bank").HasMaxLength(50).HasComment("������");
            builder.Property(table => table.Type).HasColumnName("type").HasMaxLength(1).HasDefaultValue(0).HasComment("������: 0 ��� 1 ���ÿ�");
            builder.Property(table => table.CardNo).HasColumnName("card_no").HasMaxLength(30).HasComment("������");
            builder.Property(table => table.ExpiryDate).HasColumnName("expiry_date").HasMaxLength(30).HasDefaultValue(string.Empty).HasComment("����Ч��");
            builder.Property(table => table.Status).HasColumnName("status").HasMaxLength(2).HasDefaultValue(0).HasComment("���״̬");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
