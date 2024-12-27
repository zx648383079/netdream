using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Migrations
{
    public class GoodsEntityTypeConfiguration : IEntityTypeConfiguration<GoodsEntity>
    {
        public void Configure(EntityTypeBuilder<GoodsEntity> builder)
        {
            builder.ToTable("Goods", table => table.HasComment(""));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.CatId).HasColumnName("cat_id");
            builder.Property(table => table.BrandId).HasColumnName("brand_id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(100).HasComment("��Ʒ��");
            builder.Property(table => table.SeriesNumber).HasColumnName("series_number").HasMaxLength(100);
            builder.Property(table => table.Keywords).HasColumnName("keywords").HasMaxLength(200).HasComment("�ؼ���");
            builder.Property(table => table.Thumb).HasColumnName("thumb").HasMaxLength(200).HasComment("����ͼ");
            builder.Property(table => table.Picture).HasColumnName("picture").HasMaxLength(200).HasComment("��ͼ");
            builder.Property(table => table.Description).HasColumnName("description").HasMaxLength(200).HasComment("�ؼ���");
            builder.Property(table => table.Brief).HasColumnName("brief").HasMaxLength(200).HasComment("���");
            builder.Property(table => table.Content).HasColumnName("content").HasComment("����");
            builder.Property(table => table.Price).HasColumnName("price").HasMaxLength(8).HasDefaultValue(0).HasComment("���ۼ�");
            builder.Property(table => table.MarketPrice).HasColumnName("market_price").HasMaxLength(8).HasDefaultValue(0).HasComment("ԭ��/�г���");
            builder.Property(table => table.CostPrice).HasColumnName("cost_price").HasMaxLength(8).HasDefaultValue(0).HasComment("�ɱ���");
            builder.Property(table => table.Stock).HasColumnName("stock").HasDefaultValue(1);
            builder.Property(table => table.AttributeGroupId).HasColumnName("attribute_group_id").HasDefaultValue(0);
            builder.Property(table => table.Weight).HasColumnName("weight").HasMaxLength(8).HasDefaultValue(0);
            builder.Property(table => table.ShippingId).HasColumnName("shipping_id").HasDefaultValue(0).HasComment("���ͷ�ʽ");
            builder.Property(table => table.Sales).HasColumnName("sales").HasDefaultValue(0).HasComment("����");
            builder.Property(table => table.IsBest).HasColumnName("is_best").HasDefaultValue(0);
            builder.Property(table => table.IsHot).HasColumnName("is_hot").HasDefaultValue(0);
            builder.Property(table => table.IsNew).HasColumnName("is_new").HasDefaultValue(0);
            builder.Property(table => table.Status).HasColumnName("status").HasMaxLength(2).HasDefaultValue(10);
            builder.Property(table => table.AdminNote).HasColumnName("admin_note").HasDefaultValue(string.Empty).HasComment("����Ա��ע��ֻ��̨��ʾ");
            builder.Property(table => table.Type).HasColumnName("type").HasMaxLength(1).HasDefaultValue(0).HasComment("��Ʒ����");
            builder.Property(table => table.Position).HasColumnName("position").HasMaxLength(1).HasDefaultValue(99).HasComment("����");
            builder.Property(table => table.DynamicPosition).HasColumnName("dynamic_position").HasMaxLength(1)
                .HasDefaultValue(0).HasComment("��̬����");
            builder.Property(table => table.DeletedAt).HasColumnName("deleted_at");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
