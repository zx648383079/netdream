using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Migrations
{
    public class InvoiceTitleEntityTypeConfiguration : IEntityTypeConfiguration<InvoiceTitleEntity>
    {
        public void Configure(EntityTypeBuilder<InvoiceTitleEntity> builder)
        {
            builder.ToTable("InvoiceTitle", table => table.HasComment("�û���Ʊ̧ͷ"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.TitleType).HasColumnName("title_type").HasMaxLength(1).HasDefaultValue(0).HasComment("��Ʊ̧ͷ����");
            builder.Property(table => table.Type).HasColumnName("type").HasMaxLength(1).HasDefaultValue(0).HasComment("��Ʊ����");
            builder.Property(table => table.Title).HasColumnName("title").HasMaxLength(100).HasComment("̧ͷ");
            builder.Property(table => table.TaxNo).HasColumnName("tax_no").HasMaxLength(20).HasComment("˰��ǼǺ�");
            builder.Property(table => table.Tel).HasColumnName("tel").HasMaxLength(11).HasComment("ע�᳡���绰");
            builder.Property(table => table.Bank).HasColumnName("bank").HasMaxLength(100).HasComment("��������");
            builder.Property(table => table.Account).HasColumnName("account").HasMaxLength(60).HasComment("���������˺�");
            builder.Property(table => table.Address).HasColumnName("address").HasComment("ע�᳡����ַ");
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
