using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Migrations
{
    public class CertificationEntityTypeConfiguration : IEntityTypeConfiguration<CertificationEntity>
    {
        public void Configure(EntityTypeBuilder<CertificationEntity> builder)
        {
            builder.ToTable("Certification", table => table.HasComment("�û�ʵ����"));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(20).HasComment("��ʵ����");
            builder.Property(table => table.Sex).HasColumnName("sex").HasMaxLength(0).HasDefaultValue("��").HasComment("�Ա�");
            builder.Property(table => table.Country).HasColumnName("country").HasMaxLength(20).HasDefaultValue(string.Empty).HasComment("���һ����");
            builder.Property(table => table.Type).HasColumnName("type").HasMaxLength(1).HasDefaultValue(0).HasComment("֤������");
            builder.Property(table => table.CardNo).HasColumnName("card_no").HasMaxLength(30).HasComment("֤������");
            builder.Property(table => table.ExpiryDate).HasColumnName("expiry_date").HasMaxLength(30).HasDefaultValue(string.Empty).HasComment("֤����Ч��");
            builder.Property(table => table.Profession).HasColumnName("profession").HasMaxLength(30).HasDefaultValue(string.Empty).HasComment("��ҵ");
            builder.Property(table => table.Address).HasColumnName("address").HasMaxLength(200).HasDefaultValue(string.Empty).HasComment("��ַ");
            builder.Property(table => table.FrontSide).HasColumnName("front_side").HasMaxLength(200).HasDefaultValue(string.Empty).HasComment("������");
            builder.Property(table => table.BackSide).HasColumnName("back_side").HasMaxLength(200).HasDefaultValue(string.Empty).HasComment("������");
            builder.Property(table => table.Status).HasColumnName("status").HasMaxLength(2).HasDefaultValue(0).HasComment("���״̬");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
