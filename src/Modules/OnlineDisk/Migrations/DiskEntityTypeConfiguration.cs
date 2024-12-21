using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.OnlineDisk.Entities;

namespace NetDream.Modules.OnlineDisk.Migrations
{
    public class DiskEntityTypeConfiguration : IEntityTypeConfiguration<DiskEntity>
    {
        public void Configure(EntityTypeBuilder<DiskEntity> builder)
        {
            builder.ToTable("disk", table => table.HasComment(""));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(100);
            builder.Property(table => table.Extension).HasColumnName("extension").HasMaxLength(20);
            builder.Property(table => table.FileId).HasColumnName("file_id").HasDefaultValue(0);
            builder.Property(table => table.UserId).HasColumnName("user_id");
            builder.Property(table => table.LeftId).HasColumnName("left_id").HasDefaultValue(0);
            builder.Property(table => table.RightId).HasColumnName("right_id").HasDefaultValue(0);
            builder.Property(table => table.ParentId).HasColumnName("parent_id").HasDefaultValue(0);
            builder.Property(table => table.DeletedAt).HasColumnName("deleted_at");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
