using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Migrations
{
    public class InvoiceTitleEntityTypeConfiguration : IEntityTypeConfiguration<InvoiceTitleEntity>
    {
        public void Configure(EntityTypeBuilder<InvoiceTitleEntity> builder)
        {
            builder.ToTable("InvoiceTitle", table => table.HasComment("用户发票抬头"));
            builder.HasKey(i => i.Id);
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
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
