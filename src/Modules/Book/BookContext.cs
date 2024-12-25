using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Book.Entities;
using NetDream.Modules.Book.Migrations;
using NetDream.Shared.Providers.Context;
using NetDream.Shared.Providers.Entities;
using NetDream.Shared.Providers.Migrations;

namespace NetDream.Modules.Book
{
    public class BookContext(DbContextOptions<BookContext> options): 
        DbContext(options), IMetaContext, IDayLogContext, ITagContext
    {
        public DbSet<AuthorEntity> Authors {get; set; }
        public DbSet<BookEntity> Books {get; set; }
        public DbSet<MetaEntity> Metas {get; set; }
        public DbSet<BuyLogEntity> BuyLogs {get; set; }
        public DbSet<CategoryEntity> Categories {get; set; }
        public DbSet<ChapterBodyEntity> ChapterBodies {get; set; }
        public DbSet<ChapterEntity> Chapters {get; set; }
        public DbSet<DayLogEntity> DayLogs {get; set; }
        public DbSet<HistoryEntity> Histories {get; set; }
        public DbSet<ListEntity> Lists {get; set; }
        public DbSet<ListItemEntity> ListItems {get; set; }
        public DbSet<LogEntity> Logs {get; set; }
        public DbSet<RoleEntity> Roles {get; set; }
        public DbSet<RoleRelationEntity> RoleRelations {get; set; }
        public DbSet<SourceEntity> Sources {get; set; }
        public DbSet<SourceSiteEntity> SourceSites {get; set; }
        public DbSet<TagEntity> Tags { get; set; }
        public DbSet<TagLinkEntity> TagLinks { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AuthorEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new BookEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new MetaEntityTypeConfiguration("book"));
            modelBuilder.ApplyConfiguration(new BuyLogEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ChapterBodyEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ChapterEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new DayLogEntityTypeConfiguration("book"));
            modelBuilder.ApplyConfiguration(new HistoryEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ListEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ListItemEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new LogEntityTypeConfiguration("book"));
            modelBuilder.ApplyConfiguration(new RoleEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RoleRelationEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SourceEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SourceSiteEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new TagEntityTypeConfiguration("book"));
            modelBuilder.ApplyConfiguration(new TagLinkEntityTypeConfiguration("book"));
            base.OnModelCreating(modelBuilder);
        }
    }
}
