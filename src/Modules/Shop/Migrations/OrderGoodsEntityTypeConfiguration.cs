using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Migrations
{
    public class OrderGoodsEntityTypeConfiguration : IEntityTypeConfiguration<OrderGoodsEntity>
    {
        public void Configure(EntityTypeBuilder<OrderGoodsEntity> builder)
        {
            builder.ToTable("OrderGoods", table => table.HasComment(""));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.OrderId).HasColumnName("order_id");
            builder.Property(table => table.GoodsId).HasColumnName("goods_id");
            builder.Property(table => table.ProductId).HasColumnName("product_id").HasDefaultValue(0);
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(100).HasComment("��Ʒ��");
            builder.Property(table => table.SeriesNumber).HasColumnName("series_number").HasMaxLength(100);
            builder.Property(table => table.Thumb).HasColumnName("thumb").HasMaxLength(200).HasComment("����ͼ");
            builder.Property(table => table.Amount).HasColumnName("amount").HasDefaultValue(1);
            builder.Property(table => table.Price).HasColumnName("price").HasMaxLength(8);
            builder.Property(table => table.Discount).HasColumnName("discount").HasMaxLength(8).HasDefaultValue(0)
                .HasComment("�����ܵ��ۿ�");
            builder.Property(table => table.RefundId).HasColumnName("refund_id").HasDefaultValue(0);
            builder.Property(table => table.Status).HasColumnName("status").HasDefaultValue(0);
            builder.Property(table => table.AfterSaleStatus).HasColumnName("after_sale_status").HasDefaultValue(0);
            builder.Property(table => table.CommentId).HasColumnName("comment_id").HasDefaultValue(0).HasComment("����id");
            builder.Property(table => table.TypeRemark).HasColumnName("type_remark").HasDefaultValue(string.Empty).HasComment("��Ʒ���ͱ�ע��Ϣ");
        }
    }
}
