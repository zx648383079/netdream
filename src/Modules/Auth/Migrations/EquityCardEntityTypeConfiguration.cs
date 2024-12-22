using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Auth.Entities;

namespace NetDream.Modules.Auth.Migrations
{
    public class EquityCardEntityTypeConfiguration : IEntityTypeConfiguration<EquityCardEntity>
    {
        public void Configure(EntityTypeBuilder<EquityCardEntity> builder)
        {
            builder.ToTable("equity_card", table => table.HasComment("�����޵�Ȩ�濨"));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(32);
            builder.Property(table => table.Icon).HasColumnName("icon");
            builder.Property(table => table.Configure).HasColumnName("configure").HasMaxLength(200).HasDefaultValue(string.Empty);
            builder.Property(table => table.Status).HasColumnName("status").HasMaxLength(2).HasDefaultValue(0);
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}