using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Shared.Providers.Entities;

namespace NetDream.Shared.Providers.Migrations
{
    public class FileLogEntityTypeConfiguration : IEntityTypeConfiguration<FileLogEntity>
    {
        public void Configure(EntityTypeBuilder<FileLogEntity> builder)
        {
            builder.ToTable("base_file_log", table => table.HasComment(""));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.FileId).HasColumnName("file_id");
            builder.Property(table => table.ItemType).HasColumnName("item_type").HasMaxLength(1).HasDefaultValue(0);
            builder.Property(table => table.ItemId).HasColumnName("item_id");
            builder.Property(table => table.Data).HasColumnName("data").HasDefaultValue(string.Empty);
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
