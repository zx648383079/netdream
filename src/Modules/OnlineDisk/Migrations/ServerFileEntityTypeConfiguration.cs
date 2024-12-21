using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.OnlineDisk.Entities;

namespace NetDream.Modules.OnlineDisk.Migrations
{
    public class ServerFileEntityTypeConfiguration : IEntityTypeConfiguration<ServerFileEntity>
    {
        public void Configure(EntityTypeBuilder<ServerFileEntity> builder)
        {
            builder.ToTable("disk_server_file", table => table.HasComment(""));
            builder.Property(table => table.ServerId).HasColumnName("server_id");
            builder.Property(table => table.FileId).HasColumnName("file_id");
        }
    }
}
