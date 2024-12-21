using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.MicroBlog.Entities;

namespace NetDream.Modules.MicroBlog.Migrations
{
    public class BlogTopicEntityTypeConfiguration : IEntityTypeConfiguration<BlogTopicEntity>
    {
        public void Configure(EntityTypeBuilder<BlogTopicEntity> builder)
        {
            builder.ToTable("micro_blog_topic", table => table.HasComment(""));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.MicroId).HasColumnName("micro_id");
            builder.Property(table => table.TopicId).HasColumnName("topic_id");
        }
    }
}
