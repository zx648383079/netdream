using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Migrations
{
    public class CouponEntityTypeConfiguration : IEntityTypeConfiguration<CouponEntity>
    {
        public void Configure(EntityTypeBuilder<CouponEntity> builder)
        {
            builder.ToTable("Coupon", table => table.HasComment("�Ż�ȯ"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(30);
            builder.Property(table => table.Thumb).HasColumnName("thumb").HasDefaultValue(string.Empty);
            builder.Property(table => table.Type).HasColumnName("type").HasMaxLength(2).HasDefaultValue(0).HasComment("�Ż�����");
            builder.Property(table => table.Rule).HasColumnName("rule").HasMaxLength(2).HasDefaultValue(0).HasComment("ʹ�õ���Ʒ");
            builder.Property(table => table.RuleValue).HasColumnName("rule_value").HasDefaultValue(string.Empty).HasComment("ʹ�õ���Ʒ");
            builder.Property(table => table.MinMoney).HasColumnName("min_money").HasMaxLength(8).HasDefaultValue(0).HasComment("�����ٿ���");
            builder.Property(table => table.Money).HasColumnName("money").HasMaxLength(8).HasDefaultValue(0).HasComment("���ۻ��Żݽ��");
            builder.Property(table => table.SendType).HasColumnName("send_type").HasDefaultValue(0).HasComment("��������");
            builder.Property(table => table.SendValue).HasColumnName("send_value").HasDefaultValue(0).HasComment("��������������");
            builder.Property(table => table.EveryAmount).HasColumnName("every_amount").HasDefaultValue(0).HasComment("ÿ���û�����ȡ����");
            builder.Property(table => table.StartAt).HasColumnName("start_at").HasComment("��Ч�ڿ�ʼʱ��");
            builder.Property(table => table.EndAt).HasColumnName("end_at").HasComment("��Ч�ڽ���ʱ��");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
