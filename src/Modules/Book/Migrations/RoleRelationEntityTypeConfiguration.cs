using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Book.Entities;

namespace NetDream.Modules.Book.Migrations
{
    public class RoleRelationEntityTypeConfiguration : IEntityTypeConfiguration<RoleRelationEntity>
    {
        public void Configure(EntityTypeBuilder<RoleRelationEntity> builder)
        {
            builder.ToTable("book_role_relation", table => table.HasComment("小说角色关系"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.RoleId).HasColumnName("role_id");
            builder.Property(table => table.Title).HasColumnName("title").HasMaxLength(50);
            builder.Property(table => table.RoleLink).HasColumnName("role_link");
        }
    }
}
