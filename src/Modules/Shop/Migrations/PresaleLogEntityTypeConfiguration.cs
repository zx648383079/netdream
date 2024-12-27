using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Migrations
{
    public class PresaleLogEntityTypeConfiguration : IEntityTypeConfiguration<PresaleLogEntity>
    {
        public void Configure(EntityTypeBuilder<PresaleLogEntity> builder)
        {
            builder.ToTable("PresaleLog", table => table.HasComment("Ԥ�ۼ�¼"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.ActId).HasColumnName("act_id");
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.OrderId).HasColumnName("order_id");
            builder.Property(table => table.OrderGoodsId).HasColumnName("order_goods_id");
            builder.Property(table => table.OrderAmount).HasColumnName("order_amount").HasMaxLength(8)
                .HasDefaultValue(0).HasComment("Ԥ���ܼ�");
            builder.Property(table => table.Deposit).HasColumnName("deposit").HasMaxLength(8)
                .HasDefaultValue(0).HasComment("Ԥ�۶���");
            builder.Property(table => table.FinalPayment).HasColumnName("final_payment").HasMaxLength(8)
                .HasDefaultValue(0).HasComment("Ԥ��β��");
            builder.Property(table => table.Status).HasColumnName("status").HasMaxLength(2).HasDefaultValue(0).HasComment("�ж�Ԥ�۶��������Ǹ�״̬");
            builder.Property(table => table.PredeterminedAt).HasColumnName("predetermined_at").HasComment("֧������ʱ��");
            builder.Property(table => table.FinalAt).HasColumnName("final_at").HasComment("β��֧��ʱ��");
            builder.Property(table => table.ShipAt).HasColumnName("ship_at").HasComment("����ʱ��");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
