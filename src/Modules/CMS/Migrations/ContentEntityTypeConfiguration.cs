using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.CMS.Entities;

namespace NetDream.Modules.CMS.Migrations
{
    public class ContentEntityTypeConfiguration : IEntityTypeConfiguration<ContentEntity>
    {
        public void Configure(EntityTypeBuilder<ContentEntity> builder)
        {
            builder.ToTable("Content", table => table.HasComment(""));
            builder.HasKey(i => i.Id);
        }
    }
}
