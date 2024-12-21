using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Migrations
{
    public class CertificationEntityTypeConfiguration : IEntityTypeConfiguration<CertificationEntity>
    {
        public void Configure(EntityTypeBuilder<CertificationEntity> builder)
        {
            builder.ToTable("Certification", table => table.HasComment("用户实名表"));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(20).HasComment("真实姓名");
            builder.Property(table => table.Sex).HasColumnName("sex").HasMaxLength(0).HasDefaultValue("男").HasComment("性别");
            builder.Property(table => table.Country).HasColumnName("country").HasMaxLength(20).HasDefaultValue(string.Empty).HasComment("国家或地区");
            builder.Property(table => table.Type).HasColumnName("type").HasMaxLength(1).HasDefaultValue(0).HasComment("证件类型");
            builder.Property(table => table.CardNo).HasColumnName("card_no").HasMaxLength(30).HasComment("证件号码");
            builder.Property(table => table.ExpiryDate).HasColumnName("expiry_date").HasMaxLength(30).HasDefaultValue(string.Empty).HasComment("证件有效期");
            builder.Property(table => table.Profession).HasColumnName("profession").HasMaxLength(30).HasDefaultValue(string.Empty).HasComment("行业");
            builder.Property(table => table.Address).HasColumnName("address").HasMaxLength(200).HasDefaultValue(string.Empty).HasComment("地址");
            builder.Property(table => table.FrontSide).HasColumnName("front_side").HasMaxLength(200).HasDefaultValue(string.Empty).HasComment("正面照");
            builder.Property(table => table.BackSide).HasColumnName("back_side").HasMaxLength(200).HasDefaultValue(string.Empty).HasComment("反面照");
            builder.Property(table => table.Status).HasColumnName("status").HasMaxLength(2).HasDefaultValue(0).HasComment("审核状态");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
