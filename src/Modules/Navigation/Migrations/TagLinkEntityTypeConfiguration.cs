using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Navigation.Entities;

namespace NetDream.Modules.Navigation.Migrations
{
    public class TagLinkEntityTypeConfiguration : IEntityTypeConfiguration<TagLinkEntity>
    {
        public void Configure(EntityTypeBuilder<TagLinkEntity> builder)
        {
            builder.ToTable("search_tag_link", table => table.HasComment(""));
            builder.HasKey("id");
            builder.Property(table => table.TagId).HasColumnName("tag_id");
            builder.Property(table => table.TargetId).HasColumnName("target_id");
        }
    }
}
