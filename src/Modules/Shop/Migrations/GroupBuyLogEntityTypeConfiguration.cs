using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Migrations
{
    public class GroupBuyLogEntityTypeConfiguration : IEntityTypeConfiguration<GroupBuyLogEntity>
    {
        public void Configure(EntityTypeBuilder<GroupBuyLogEntity> builder)
        {
            builder.ToTable("GroupBuyLog", table => table.HasComment("�Ź���¼"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.ActId).HasColumnName("act_id");
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.OrderId).HasColumnName("order_id");
            builder.Property(table => table.OrderGoodsId).HasColumnName("order_goods_id");
            builder.Property(table => table.Deposit).HasColumnName("deposit").HasMaxLength(8)
                .HasDefaultValue(0).HasComment("����");
            builder.Property(table => table.FinalPayment).HasColumnName("final_payment").HasMaxLength(8)
                .HasDefaultValue(0).HasComment("β��");
            builder.Property(table => table.Status).HasColumnName("status").HasMaxLength(2).HasDefaultValue(0).HasComment("�ж�Ԥ�۶��������Ǹ�״̬");
            builder.Property(table => table.PredeterminedAt).HasColumnName("predetermined_at").HasComment("֧������ʱ��");
            builder.Property(table => table.FinalAt).HasColumnName("final_at").HasComment("β��֧��ʱ��");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
