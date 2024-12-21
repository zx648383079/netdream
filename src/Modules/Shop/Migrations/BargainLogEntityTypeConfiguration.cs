using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Migrations
{
    public class BargainLogEntityTypeConfiguration : IEntityTypeConfiguration<BargainLogEntity>
    {
        public void Configure(EntityTypeBuilder<BargainLogEntity> builder)
        {
            builder.ToTable("BargainLog", table => table.HasComment("�û��￳��¼"));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.BargainId).HasColumnName("bargain_id");
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.Amount).HasColumnName("amount").HasMaxLength(8).HasDefaultValue(0).HasComment("�����ļ۸�");
            builder.Property(table => table.Price).HasColumnName("price").HasMaxLength(8).HasDefaultValue(0)
                .HasComment("����֮��ʣ��ļ۸�");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
