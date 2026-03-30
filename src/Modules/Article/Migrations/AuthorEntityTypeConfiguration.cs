using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Article.Entities;
using NetDream.Shared.Models;

namespace NetDream.Modules.Article.Migrations
{
    public class AuthorEntityTypeConfiguration : IEntityTypeConfiguration<AuthorEntity>
    {
        public void Configure(EntityTypeBuilder<AuthorEntity> builder)
        {
            builder.ToTable("author", table => table.HasComment("作者"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(50);
            builder.Property(table => table.Description).HasColumnName("description").HasDefaultValue(string.Empty);
            builder.Property(table => table.Avatar).HasColumnName("avatar").HasDefaultValue(string.Empty);
            builder.Property(table => table.UserId).HasColumnName("user_id").HasDefaultValue(0);
            builder.Property(table => table.Email).HasColumnName("email").HasDefaultValue(string.Empty);
            builder.Property(table => table.Url).HasColumnName("url").HasDefaultValue(string.Empty);
            builder.Property(table => table.Status).HasColumnName("status").HasMaxLength(1)
                .HasDefaultValue(ReviewStatus.None).HasComment("审核状态");
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
