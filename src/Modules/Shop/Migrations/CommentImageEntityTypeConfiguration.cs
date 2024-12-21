using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Shop.Entities;

namespace NetDream.Modules.Shop.Migrations
{
    public class CommentImageEntityTypeConfiguration : IEntityTypeConfiguration<CommentImageEntity>
    {
        public void Configure(EntityTypeBuilder<CommentImageEntity> builder)
        {
            builder.ToTable("CommentImage", table => table.HasComment(""));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.CommentId).HasColumnName("comment_id");
            builder.Property(table => table.Image).HasColumnName("image");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
