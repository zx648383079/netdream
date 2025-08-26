using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.OnlineDisk.Entities;
using NetDream.Modules.OnlineDisk.Repositories;

namespace NetDream.Modules.OnlineDisk.Migrations
{
    public class ShareEntityTypeConfiguration : IEntityTypeConfiguration<ShareEntity>
    {
        public void Configure(EntityTypeBuilder<ShareEntity> builder)
        {
            builder.ToTable("disk_share", table => table.HasComment(""));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(100);
            builder.Property(table => table.Mode).HasColumnName("mode").HasMaxLength(2).HasDefaultValue(ShareRepository.SHARE_PUBLIC);
            builder.Property(table => table.Password).HasColumnName("password").HasMaxLength(20).HasDefaultValue(string.Empty);
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.DeathAt).HasColumnName("death_at");
            builder.Property(table => table.ViewCount).HasColumnName("view_count").HasDefaultValue(0);
            builder.Property(table => table.DownCount).HasColumnName("down_count").HasDefaultValue(0);
            builder.Property(table => table.SaveCount).HasColumnName("save_count").HasDefaultValue(0);
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
