using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Document.Entities;
using NetDream.Modules.Document.Migrations;
using NetDream.Shared.Providers.Context;
using NetDream.Shared.Providers.Entities;
using NetDream.Shared.Providers.Migrations;

namespace NetDream.Modules.Document
{
    public class DocumentContext(DbContextOptions<DocumentContext> options): DbContext(options), ICommentContext
    {
        public DbSet<ApiEntity> Api {get; set; }
        public DbSet<CategoryEntity> Categories {get; set; }
        public DbSet<FieldEntity> Fields {get; set; }
        public DbSet<PageEntity> Pages {get; set; }
        public DbSet<ProjectEntity> Projects {get; set; }
        public DbSet<ProjectVersionEntity> ProjectVersions {get; set; }
        public DbSet<LogEntity> Logs { get; set; }
        public DbSet<CommentEntity> Comments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ApiEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new FieldEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PageEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectVersionEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new LogEntityTypeConfiguration("doc"));
            modelBuilder.ApplyConfiguration(new CommentEntityTypeConfiguration("doc"));
            base.OnModelCreating(modelBuilder);
        }
    }
}
