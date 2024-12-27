using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Migrations
{
    public class AffiliateLogEntityTypeConfiguration : IEntityTypeConfiguration<AffiliateLogEntity>
    {
        public void Configure(EntityTypeBuilder<AffiliateLogEntity> builder)
        {
            builder.ToTable("AffiliateLog", table => table.HasComment("�û�������¼��"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.ItemType).HasColumnName("item_type").HasMaxLength(1).HasDefaultValue(0).HasComment("����: 0 �û� 1 ����");
            builder.Property(table => table.ItemId).HasColumnName("item_id").HasComment("������/���Ƽ����û�");
            builder.Property(table => table.OrderSn).HasColumnName("order_sn").HasMaxLength(30).HasDefaultValue(string.Empty).HasComment("������");
            builder.Property(table => table.OrderAmount).HasColumnName("order_amount").HasMaxLength(8).HasDefaultValue(0).HasComment("�������");
            builder.Property(table => table.Money).HasColumnName("money").HasMaxLength(8).HasDefaultValue(0).HasComment("Ӷ��");
            builder.Property(table => table.Status).HasColumnName("status").HasMaxLength(2).HasDefaultValue(0).HasComment("���״̬");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
