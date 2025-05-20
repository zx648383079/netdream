using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Finance.Entities;

namespace NetDream.Modules.Finance.Migrations;
public class ConsumptionChannelEntityTypeConfiguration : IEntityTypeConfiguration<ConsumptionChannelEntity>
{
    public void Configure(EntityTypeBuilder<ConsumptionChannelEntity> builder)
    {
        builder.ToTable("finance_consumption_channel", table => table.HasComment("消费渠道"));
        builder.HasKey(table => table.Id);
        builder.Property(table => table.Id).HasColumnName("id").HasMaxLength(10);
        builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(50).HasComment("消费渠道名称");
        builder.Property(table => table.UserId).HasColumnName("user_id").HasMaxLength(10);
        builder.Property(table => table.UpdatedAt).HasColumnName("updated_at").HasMaxLength(10).HasDefaultValue(0);
        builder.Property(table => table.CreatedAt).HasColumnName("created_at").HasMaxLength(10).HasDefaultValue(0);
        
    }
}