using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Article.Entities;
using NetDream.Modules.Article.Migrations;

namespace NetDream.Modules.Article
{
    public class ArticleContext(DbContextOptions<ArticleContext> options) :
        DbContext(options)
    {
        public DbSet<ArticleEntity> Articles { get; set; }
        public DbSet<AuthorEntity> Authors { get; set; }

        public DbSet<CategoryEntity> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ArticleEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new AuthorEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryEntityTypeConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
