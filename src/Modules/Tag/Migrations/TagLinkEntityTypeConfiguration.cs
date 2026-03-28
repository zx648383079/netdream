using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Tag.Entities;

namespace NetDream.Modules.Tag.Migrations
{
    public class TagLinkEntityTypeConfiguration : IEntityTypeConfiguration<TagLinkEntity>
    {
        public void Configure(EntityTypeBuilder<TagLinkEntity> builder)
        {
            builder.ToTable("tag_link", table => table.HasComment("±êÇ©¹ØÁª"));
            builder.HasNoKey();
            builder.Property(table => table.TagId).HasColumnName("tag_id");
            builder.Property(table => table.ItemType).HasColumnName("item_type");
            builder.Property(table => table.ItemId).HasColumnName("item_id");
        }
    }
}
