using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Forum.Entities;

namespace NetDream.Modules.Forum.Migrations
{
    public class ForumClassifyEntityTypeConfiguration : IEntityTypeConfiguration<ForumClassifyEntity>
    {
        public void Configure(EntityTypeBuilder<ForumClassifyEntity> builder)
        {
            builder.ToTable("bbs_forum_classify", table => table.HasComment(""));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(20);
            builder.Property(table => table.Icon).HasColumnName("icon").HasMaxLength(100).HasDefaultValue(string.Empty);
            builder.Property(table => table.ForumId).HasColumnName("forum_id").HasDefaultValue(0);
            builder.Property(table => table.Position).HasColumnName("position").HasMaxLength(2).HasDefaultValue(99);
        }
    }
}
