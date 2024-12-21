using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.MicroBlog.Entities;

namespace NetDream.Modules.MicroBlog.Migrations
{
    public class AttachmentEntityTypeConfiguration : IEntityTypeConfiguration<AttachmentEntity>
    {
        public void Configure(EntityTypeBuilder<AttachmentEntity> builder)
        {
            builder.ToTable("micro_attachment", table => table.HasComment(""));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.MicroId).HasColumnName("micro_id");
            builder.Property(table => table.Thumb).HasColumnName("thumb");
            builder.Property(table => table.File).HasColumnName("file");
        }
    }
}
