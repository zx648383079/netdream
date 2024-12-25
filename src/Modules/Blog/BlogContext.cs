using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Blog.Entities;
using NetDream.Modules.Blog.Migrations;
using NetDream.Shared.Providers.Context;
using NetDream.Shared.Providers.Entities;
using NetDream.Shared.Providers.Migrations;

namespace NetDream.Modules.Blog
{
    public class BlogContext(DbContextOptions<BlogContext> options): 
        DbContext(options), IMetaContext, ITagContext, IDayLogContext
    {
        public DbSet<BlogEntity> Blogs {get; set; }
        public DbSet<DayLogEntity> DayLogs { get; set; }
        public DbSet<Entities.CommentEntity> Comments {get; set; }
        public DbSet<LogEntity> Logs {get; set; }
        public DbSet<MetaEntity> Metas {get; set; }
        public DbSet<TagEntity> Tags {get; set; }
        public DbSet<TagLinkEntity> TagLinks {get; set; }
        public DbSet<CategoryEntity> Categories {get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BlogEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new DayLogEntityTypeConfiguration("blog"));
            modelBuilder.ApplyConfiguration(new Migrations.CommentEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new LogEntityTypeConfiguration("blog"));
            modelBuilder.ApplyConfiguration(new MetaEntityTypeConfiguration("blog"));
            modelBuilder.ApplyConfiguration(new TagEntityTypeConfiguration("blog"));
            modelBuilder.ApplyConfiguration(new TagLinkEntityTypeConfiguration("blog"));
            modelBuilder.ApplyConfiguration(new TermEntityTypeConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
