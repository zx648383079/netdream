using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Book.Entities;
using NetDream.Modules.Book.Migrations;

namespace NetDream.Modules.Book
{
    public class BookContext(DbContextOptions<BookContext> options): DbContext(options)
    {
        public DbSet<AuthorEntity> Authors {get; set; }
        public DbSet<BookEntity> Books {get; set; }
        public DbSet<BookMetaEntity> BookMetas {get; set; }
        public DbSet<BuyLogEntity> BuyLogs {get; set; }
        public DbSet<CategoryEntity> Categorys {get; set; }
        public DbSet<ChapterBodyEntity> ChapterBodys {get; set; }
        public DbSet<ChapterEntity> Chapters {get; set; }
        public DbSet<ClickLogEntity> ClickLogs {get; set; }
        public DbSet<HistoryEntity> Historys {get; set; }
        public DbSet<ListEntity> Lists {get; set; }
        public DbSet<ListItemEntity> ListItems {get; set; }
        public DbSet<LogEntity> Logs {get; set; }
        public DbSet<RoleEntity> Roles {get; set; }
        public DbSet<RoleRelationEntity> RoleRelations {get; set; }
        public DbSet<SourceEntity> Sources {get; set; }
        public DbSet<SourceSiteEntity> SourceSites {get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AuthorEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new BookEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new BookMetaEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new BuyLogEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ChapterBodyEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ChapterEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ClickLogEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new HistoryEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ListEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ListItemEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new LogEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RoleEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RoleRelationEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SourceEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SourceSiteEntityTypeConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
