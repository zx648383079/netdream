using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Bot.Entities;
using NetDream.Modules.Bot.Migrations;

namespace NetDream.Modules.Bot
{
    public class BotContext(DbContextOptions<BotContext> options): DbContext(options)
    {
        public DbSet<BotEntity> Bots {get; set; }
        public DbSet<EditorCategoryEntity> EditorCategories {get; set; }
        public DbSet<EditorTemplateEntity> EditorTemplates {get; set; }
        public DbSet<MediaEntity> Medias {get; set; }
        public DbSet<MenuEntity> Menus {get; set; }
        public DbSet<MessageHistoryEntity> MessageHistories {get; set; }
        public DbSet<QrcodeEntity> Qrcode {get; set; }
        public DbSet<ReplyEntity> Replies {get; set; }
        public DbSet<TemplateEntity> Templates {get; set; }
        public DbSet<UserEntity> Users {get; set; }
        public DbSet<UserGroupEntity> UserGroups {get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BotEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new EditorTemplateCategoryEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new EditorTemplateEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new MediaEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new MenuEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new MessageHistoryEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new QrcodeEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ReplyEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new TemplateEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserGroupEntityTypeConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
