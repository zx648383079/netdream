using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Blog.Entities;
using NetDream.Modules.Blog.Migrations;

namespace NetDream.Modules.Blog
{
    public class BlogContext(DbContextOptions<BlogContext> options): DbContext(options)
    {
        public DbSet<BlogEntity> Blogs {get; set; }
        public DbSet<ClickLogEntity> ClickLogs {get; set; }
        public DbSet<CommentEntity> Comments {get; set; }
        public DbSet<LogEntity> Logs {get; set; }
        public DbSet<MetaEntity> Metas {get; set; }
        public DbSet<TagEntity> Tags {get; set; }
        public DbSet<TagRelationshipEntity> TagRelationships {get; set; }
        public DbSet<TermEntity> Terms {get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BlogEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ClickLogEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CommentEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new LogEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new MetaEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new TagEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new TagRelationshipEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new TermEntityTypeConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
