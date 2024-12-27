using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.OnlineDisk.Entities;

namespace NetDream.Modules.OnlineDisk.Migrations
{
    public class ShareFileEntityTypeConfiguration : IEntityTypeConfiguration<ShareFileEntity>
    {
        public void Configure(EntityTypeBuilder<ShareFileEntity> builder)
        {
            builder.ToTable("disk_share_file", table => table.HasComment(""));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.DiskId).HasColumnName("disk_id");
            builder.Property(table => table.ShareId).HasColumnName("share_id");
        }
    }
}
