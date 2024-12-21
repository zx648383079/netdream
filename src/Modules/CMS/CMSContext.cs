using Microsoft.EntityFrameworkCore;
using NetDream.Modules.CMS.Entities;
using NetDream.Modules.CMS.Migrations;

namespace NetDream.Modules.CMS
{
    public class CMSContext(DbContextOptions<CMSContext> options): DbContext(options)
    {
        public DbSet<CategoryEntity> Categorys {get; set; }
        public DbSet<CommentEntity> Comments {get; set; }
        public DbSet<ContentEntity> Contents {get; set; }
        public DbSet<GroupEntity> Groups {get; set; }
        public DbSet<LinkageDataEntity> LinkageDatas {get; set; }
        public DbSet<LinkageEntity> Linkages {get; set; }
        public DbSet<LogEntity> Logs {get; set; }
        public DbSet<ModelEntity> Models {get; set; }
        public DbSet<ModelFieldEntity> ModelFields {get; set; }
        public DbSet<RecycleBinEntity> RecycleBins {get; set; }
        public DbSet<SiteEntity> Sites {get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CategoryEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CommentEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ContentEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new GroupEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new LinkageDataEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new LinkageEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new LogEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ModelEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ModelFieldEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RecycleBinEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SiteEntityTypeConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
