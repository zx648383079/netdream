using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Shared.Providers.Entities;

namespace NetDream.Shared.Providers.Migrations
{
    public class FileQuoteEntityTypeConfiguration : IEntityTypeConfiguration<FileQuoteEntity>
    {
        public void Configure(EntityTypeBuilder<FileQuoteEntity> builder)
        {
            builder.ToTable("base_file_quote", table => table.HasComment(""));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.FileId).HasColumnName("file_id");
            builder.Property(table => table.ItemType).HasColumnName("item_type").HasMaxLength(1).HasDefaultValue(0);
            builder.Property(table => table.ItemId).HasColumnName("item_id");
            builder.Property(table => table.UserId).HasColumnName("user_id").HasDefaultValue(0);
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
