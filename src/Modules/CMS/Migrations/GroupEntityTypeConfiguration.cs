using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.CMS.Entities;

namespace NetDream.Modules.CMS.Migrations
{
    public class GroupEntityTypeConfiguration : IEntityTypeConfiguration<GroupEntity>
    {
        public void Configure(EntityTypeBuilder<GroupEntity> builder)
        {
            builder.ToTable("Group", table => table.HasComment(""));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(20);
            builder.Property(table => table.Type).HasColumnName("type").HasMaxLength(2).HasDefaultValue(0);
            builder.Property(table => table.Description).HasColumnName("description").HasDefaultValue(string.Empty);
        }
    }
}
