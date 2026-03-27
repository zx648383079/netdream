using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Blog.Entities;
using NetDream.Modules.Blog.Migrations;
using NetDream.Shared.Providers.Context;
using NetDream.Shared.Providers.Entities;
using NetDream.Shared.Providers.Migrations;

namespace NetDream.Modules.Blog
{
    public class BlogContext(DbContextOptions<BlogContext> options): 
        DbContext(options), IMetaContext
    {
        public DbSet<BlogEntity> Blogs {get; set; }
        public DbSet<MetaEntity> Metas {get; set; }

        public DbSet<CategoryEntity> Categories {get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BlogEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new MetaEntityTypeConfiguration("blog"));
            modelBuilder.ApplyConfiguration(new TermEntityTypeConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
