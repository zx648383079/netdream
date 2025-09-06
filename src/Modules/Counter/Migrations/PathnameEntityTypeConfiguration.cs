using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Counter.Entities;

namespace NetDream.Modules.Counter.Migrations
{
    public class PathnameEntityTypeConfiguration : IEntityTypeConfiguration<PathnameEntity>
    {
        public void Configure(EntityTypeBuilder<PathnameEntity> builder)
        {
            builder.ToTable("ctr_pathname", table => table.HasComment("Â·¾¶"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Name).HasColumnName("name");
        }
    }
}
