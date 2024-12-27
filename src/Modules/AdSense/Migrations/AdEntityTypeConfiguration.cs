using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.AdSense.Entities;
using NetDream.Modules.AdSense.Repositories;

namespace NetDream.Modules.AdSense.Migrations
{
    public class AdEntityTypeConfiguration : IEntityTypeConfiguration<AdEntity>
    {
        public void Configure(EntityTypeBuilder<AdEntity> builder)
        {
            builder.ToTable("ad");
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(30);
            builder.Property(table => table.PositionId).HasColumnName("position_id");
            builder.Property(table => table.Type).HasColumnName("type").HasMaxLength(1).HasDefaultValue(AdRepository.TYPE_TEXT);
            builder.Property(table => table.Url).HasColumnName("url");
            builder.Property(table => table.Content).HasColumnName("content");
            builder.Property(table => table.StartAt).HasColumnName("start_at");
            builder.Property(table => table.EndAt).HasColumnName("end_at");
            builder.Property(table => table.Status).HasColumnName("status").HasDefaultValue(1).HasComment("是否启用广告");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
