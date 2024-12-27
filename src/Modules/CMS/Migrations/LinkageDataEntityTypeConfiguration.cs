using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.CMS.Entities;

namespace NetDream.Modules.CMS.Migrations
{
    public class LinkageDataEntityTypeConfiguration : IEntityTypeConfiguration<LinkageDataEntity>
    {
        public void Configure(EntityTypeBuilder<LinkageDataEntity> builder)
        {
            builder.ToTable("LinkageData", table => table.HasComment(""));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.LinkageId).HasColumnName("linkage_id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(100);
            builder.Property(table => table.ParentId).HasColumnName("parent_id").HasDefaultValue(0);
            builder.Property(table => table.Position).HasColumnName("position").HasMaxLength(2).HasDefaultValue(99);
            builder.Property(table => table.Description).HasColumnName("description").HasDefaultValue(string.Empty);
            builder.Property(table => table.Thumb).HasColumnName("thumb").HasDefaultValue(string.Empty);
            builder.Property(table => table.FullName).HasColumnName("full_name").HasMaxLength(200).HasDefaultValue(string.Empty);
        }
    }
}
