using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Migrations
{
    public class InvoiceEntityTypeConfiguration : IEntityTypeConfiguration<InvoiceEntity>
    {
        public void Configure(EntityTypeBuilder<InvoiceEntity> builder)
        {
            builder.ToTable("Invoice", table => table.HasComment("开票记录"));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.TitleType).HasColumnName("title_type").HasMaxLength(1).HasDefaultValue(0).HasComment("发票抬头类型");
            builder.Property(table => table.Type).HasColumnName("type").HasMaxLength(1).HasDefaultValue(0).HasComment("发票类型");
            builder.Property(table => table.Title).HasColumnName("title").HasMaxLength(100).HasComment("抬头");
            builder.Property(table => table.TaxNo).HasColumnName("tax_no").HasMaxLength(20).HasComment("税务登记号");
            builder.Property(table => table.Tel).HasColumnName("tel").HasMaxLength(11).HasComment("注册场所电话");
            builder.Property(table => table.Bank).HasColumnName("bank").HasMaxLength(100).HasComment("开户银行");
            builder.Property(table => table.Account).HasColumnName("account").HasMaxLength(60).HasComment("基本开户账号");
            builder.Property(table => table.Address).HasColumnName("address").HasComment("注册场所地址");
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.Money).HasColumnName("money").HasMaxLength(10).HasDefaultValue(0).HasComment("开票金额");
            builder.Property(table => table.Status).HasColumnName("status").HasMaxLength(2).HasDefaultValue(0).HasComment("开票状态");
            builder.Property(table => table.InvoiceType).HasColumnName("invoice_type").HasDefaultValue(0).HasComment("电子发票/纸质发票");
            builder.Property(table => table.ReceiverEmail).HasColumnName("receiver_email").HasMaxLength(100).HasDefaultValue(string.Empty);
            builder.Property(table => table.ReceiverName).HasColumnName("receiver_name").HasMaxLength(100).HasDefaultValue(string.Empty);
            builder.Property(table => table.ReceiverTel).HasColumnName("receiver_tel").HasMaxLength(100).HasDefaultValue(string.Empty);
            builder.Property(table => table.ReceiverRegionId).HasColumnName("receiver_region_id").HasDefaultValue(0);
            builder.Property(table => table.ReceiverRegionName).HasColumnName("receiver_region_name").HasDefaultValue(string.Empty);
            builder.Property(table => table.ReceiverAddress).HasColumnName("receiver_address").HasDefaultValue(string.Empty);
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
