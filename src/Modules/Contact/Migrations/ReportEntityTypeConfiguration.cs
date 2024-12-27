using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Contact.Entities;

namespace NetDream.Modules.Contact.Migrations
{
    public class ReportEntityTypeConfiguration : IEntityTypeConfiguration<ReportEntity>
    {
        public void Configure(EntityTypeBuilder<ReportEntity> builder)
        {
            builder.ToTable("cif_report", table => table.HasComment("举报和投诉"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Email).HasColumnName("email").HasMaxLength(50).HasDefaultValue(string.Empty);
            builder.Property(table => table.ItemType).HasColumnName("item_type").HasMaxLength(1).HasDefaultValue(0);
            builder.Property(table => table.ItemId).HasColumnName("item_id").HasDefaultValue(0);
            builder.Property(table => table.Type).HasColumnName("type").HasMaxLength(1).HasDefaultValue(0);
            builder.Property(table => table.Title).HasColumnName("title").HasDefaultValue(string.Empty);
            builder.Property(table => table.Content).HasColumnName("content").HasDefaultValue(string.Empty);
            builder.Property(table => table.Files).HasColumnName("files").HasDefaultValue(string.Empty);
            builder.Property(table => table.Status).HasColumnName("status").HasDefaultValue(0);
            builder.Property(table => table.UserId).HasColumnName("user_id").HasDefaultValue(0);
            builder.Property(table => table.Ip).HasColumnName("ip").HasMaxLength(120).HasDefaultValue(string.Empty);
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
