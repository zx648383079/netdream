using Microsoft.EntityFrameworkCore;
using NetDream.Modules.SEO.Entities;
using NetDream.Modules.SEO.Migrations;

namespace NetDream.Modules.SEO
{
    public class SEOContext(DbContextOptions<SEOContext> options): DbContext(options)
    {
        public DbSet<AgreementEntity> Agreements {get; set; }
        public DbSet<BlackWordEntity> BlackWords {get; set; }
        public DbSet<EmojiCategoryEntity> EmojiCategories {get; set; }
        public DbSet<EmojiEntity> Emojis {get; set; }
        public DbSet<FileEntity> Files {get; set; }
        public DbSet<FileLogEntity> FileLogs {get; set; }
        public DbSet<FileQuoteEntity> FileQuotes {get; set; }
        public DbSet<OptionEntity> Options {get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AgreementEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new BlackWordEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new EmojiCategoryEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new EmojiEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new FileEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new FileLogEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new FileQuoteEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OptionEntityTypeConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
