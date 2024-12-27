using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Shared.Providers.Entities;

namespace NetDream.Shared.Providers.Migrations
{
    public class FileEntityTypeConfiguration : IEntityTypeConfiguration<FileEntity>
    {
        public void Configure(EntityTypeBuilder<FileEntity> builder)
        {
            builder.ToTable("base_file", table => table.HasComment(""));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(100);
            builder.Property(table => table.Extension).HasColumnName("extension").HasMaxLength(10);
            builder.Property(table => table.Md5).HasColumnName("md5").HasMaxLength(32);//.Unique();
            builder.Property(table => table.Path).HasColumnName("path").HasMaxLength(200);
            builder.Property(table => table.Folder).HasColumnName("folder").HasMaxLength(2).HasDefaultValue(1);
            builder.Property(table => table.Size).HasColumnName("size").HasDefaultValue("0");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
