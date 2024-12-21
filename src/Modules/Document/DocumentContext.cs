using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Document.Entities;
using NetDream.Modules.Document.Migrations;

namespace NetDream.Modules.Document
{
    public class DocumentContext(DbContextOptions<DocumentContext> options): DbContext(options)
    {
        public DbSet<ApiEntity> Apis {get; set; }
        public DbSet<CategoryEntity> Categorys {get; set; }
        public DbSet<FieldEntity> Fields {get; set; }
        public DbSet<PageEntity> Pages {get; set; }
        public DbSet<ProjectEntity> Projects {get; set; }
        public DbSet<ProjectVersionEntity> ProjectVersions {get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ApiEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new FieldEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PageEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectVersionEntityTypeConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
