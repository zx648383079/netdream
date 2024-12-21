using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.OnlineDisk.Entities;

namespace NetDream.Modules.OnlineDisk.Migrations
{
    public class ClientFileEntityTypeConfiguration : IEntityTypeConfiguration<ClientFileEntity>
    {
        public void Configure(EntityTypeBuilder<ClientFileEntity> builder)
        {
            builder.ToTable("disk_client_file", table => table.HasComment(""));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(100);
            builder.Property(table => table.Extension).HasColumnName("extension").HasMaxLength(20);
            builder.Property(table => table.Md5).HasColumnName("md5").HasMaxLength(32);
            builder.Property(table => table.Location).HasColumnName("location").HasMaxLength(200);
            builder.Property(table => table.Size).HasColumnName("size").HasMaxLength(50).HasDefaultValue(0);
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
