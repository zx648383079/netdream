using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.OnlineDisk.Entities;

namespace NetDream.Modules.OnlineDisk.Migrations
{
    public class ShareUserEntityTypeConfiguration : IEntityTypeConfiguration<ShareUserEntity>
    {
        public void Configure(EntityTypeBuilder<ShareUserEntity> builder)
        {
            builder.ToTable("disk_share_user", table => table.HasComment(""));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.ShareId).HasColumnName("share_id");
        }
    }
}
