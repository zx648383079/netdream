using Microsoft.EntityFrameworkCore;
using NetDream.Modules.MicroBlog.Entities;
using NetDream.Modules.MicroBlog.Migrations;
using NetDream.Shared.Providers.Context;
using NetDream.Shared.Providers.Entities;
using NetDream.Shared.Providers.Migrations;

namespace NetDream.Modules.MicroBlog
{
    public class MicroBlogContext(DbContextOptions<MicroBlogContext> options): DbContext(options), ICommentContext
    {
        public DbSet<AttachmentEntity> Attachments {get; set; }
        public DbSet<BlogEntity> Blogs {get; set; }
        public DbSet<BlogTopicEntity> BlogTopics {get; set; }
        public DbSet<CommentEntity> Comments {get; set; }
        public DbSet<LogEntity> Logs {get; set; }
        public DbSet<TopicEntity> Topics {get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AttachmentEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new BlogEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new BlogTopicEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CommentEntityTypeConfiguration("micro"));
            modelBuilder.ApplyConfiguration(new LogEntityTypeConfiguration("micro"));
            modelBuilder.ApplyConfiguration(new TopicEntityTypeConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
