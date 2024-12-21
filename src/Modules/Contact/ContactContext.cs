using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Contact.Entities;
using NetDream.Modules.Contact.Migrations;

namespace NetDream.Modules.Contact
{
    public class ContactContext(DbContextOptions<ContactContext> options): DbContext(options)
    {
        public DbSet<FeedbackEntity> Feedbacks {get; set; }
        public DbSet<FriendLinkEntity> FriendLinks {get; set; }
        public DbSet<ReportEntity> Reports {get; set; }
        public DbSet<SubscribeEntity> Subscribes {get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FeedbackEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new FriendLinkEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ReportEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SubscribeEntityTypeConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
