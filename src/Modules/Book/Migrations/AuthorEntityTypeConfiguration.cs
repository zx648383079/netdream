using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Book.Entities;

namespace NetDream.Modules.Book.Migrations
{
    public class AuthorEntityTypeConfiguration : IEntityTypeConfiguration<AuthorEntity>
    {
        public void Configure(EntityTypeBuilder<AuthorEntity> builder)
        {
            builder.ToTable("book_author", table => table.HasComment("小说作者"));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.HasIndex("name").IsUnique();
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(100).HasComment("作者名");
            builder.Property(table => table.Avatar).HasColumnName("avatar").HasMaxLength(200).HasDefaultValue(string.Empty).HasComment("作者头像");
            builder.Property(table => table.Description).HasColumnName("description").HasMaxLength(200).HasDefaultValue(string.Empty).HasComment("简介");
            builder.Property(table => table.UserId).HasColumnName("user_id").HasDefaultValue(0);
            builder.Property(table => table.Status).HasColumnName("status").HasMaxLength(2).HasDefaultValue(0);
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");

            builder.HasMany(p => p.Books)
                      .WithOne(b => b.Author)
                      .HasPrincipalKey(p => p.Id)
                      .HasForeignKey(b => b.AuthorId);
        }
    }
}
