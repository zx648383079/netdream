using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Shared.Providers.Entities;

namespace NetDream.Shared.Providers.Migrations
{
    public class TagEntityTypeConfiguration(string prefix) : IEntityTypeConfiguration<TagEntity>
    {
        public void Configure(EntityTypeBuilder<TagEntity> builder)
        {
            builder.ToTable(prefix + "_tag", table => table.HasComment("±êÇ©"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(20);
        }
    }
}
