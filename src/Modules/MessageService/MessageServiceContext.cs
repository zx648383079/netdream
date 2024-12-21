using Microsoft.EntityFrameworkCore;
using NetDream.Modules.MessageService.Entities;
using NetDream.Modules.MessageService.Migrations;

namespace NetDream.Modules.MessageService
{
    public class MessageServiceContext(DbContextOptions<MessageServiceContext> options): DbContext(options)
    {
        public DbSet<LogEntity> Logs {get; set; }
        public DbSet<TemplateEntity> Templates {get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new LogEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new TemplateEntityTypeConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
