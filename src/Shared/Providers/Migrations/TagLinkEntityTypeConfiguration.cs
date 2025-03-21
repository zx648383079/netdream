using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Shared.Providers.Entities;

namespace NetDream.Shared.Providers.Migrations
{
    public class TagLinkEntityTypeConfiguration(string prefix) : IEntityTypeConfiguration<TagLinkEntity>
    {
        public void Configure(EntityTypeBuilder<TagLinkEntity> builder)
        {
            builder.ToTable(prefix + "_tag_link", table => table.HasComment("±êÇ©¹ØÁª"));
            builder.HasNoKey();
            builder.Property(table => table.TagId).HasColumnName("tag_id");
            builder.Property(table => table.TargetId).HasColumnName("target_id");
        }
    }
}
