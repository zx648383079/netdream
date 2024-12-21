using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.OnlineDisk.Entities;

namespace NetDream.Modules.OnlineDisk.Migrations
{
    public class ServerEntityTypeConfiguration : IEntityTypeConfiguration<ServerEntity>
    {
        public void Configure(EntityTypeBuilder<ServerEntity> builder)
        {
            builder.ToTable("disk_server", table => table.HasComment(""));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Token).HasColumnName("token");
            builder.Property(table => table.Ip).HasColumnName("ip").HasMaxLength(120);
            builder.Property(table => table.Port).HasColumnName("port").HasMaxLength(6);
            builder.Property(table => table.CanUpload).HasColumnName("can_upload");
            builder.Property(table => table.UploadUrl).HasColumnName("upload_url");
            builder.Property(table => table.DownloadUrl).HasColumnName("download_url");
            builder.Property(table => table.PingUrl).HasColumnName("ping_url");
            builder.Property(table => table.FileCount).HasColumnName("file_count");
            builder.Property(table => table.Status).HasColumnName("status").HasMaxLength(1).HasDefaultValue(0);
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
