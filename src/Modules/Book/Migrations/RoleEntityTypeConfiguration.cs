using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Book.Entities;

namespace NetDream.Modules.Book.Migrations
{
    public class RoleEntityTypeConfiguration : IEntityTypeConfiguration<RoleEntity>
    {
        public void Configure(EntityTypeBuilder<RoleEntity> builder)
        {
            builder.ToTable("book_role", table => table.HasComment("小说角色"));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.BookId).HasColumnName("book_id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(50);
            builder.Property(table => table.Avatar).HasColumnName("avatar").HasMaxLength(200).HasDefaultValue(string.Empty);
            builder.Property(table => table.Description).HasColumnName("description").HasMaxLength(200).HasDefaultValue(string.Empty);
            builder.Property(table => table.Character).HasColumnName("character").HasMaxLength(20).HasDefaultValue(string.Empty).HasComment("身份：主角或");
            builder.Property(table => table.X).HasColumnName("x").HasMaxLength(20).HasDefaultValue(string.Empty);
            builder.Property(table => table.Y).HasColumnName("y").HasMaxLength(20).HasDefaultValue(string.Empty);
        }
    }
}
