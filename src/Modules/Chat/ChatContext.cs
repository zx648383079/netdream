using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Chat.Entities;
using NetDream.Modules.Chat.Migrations;

namespace NetDream.Modules.Chat
{
    public class ChatContext(DbContextOptions<ChatContext> options): DbContext(options)
    {
        public DbSet<ApplyEntity> Applys {get; set; }
        public DbSet<FriendClassifyEntity> FriendClassifys {get; set; }
        public DbSet<FriendEntity> Friends {get; set; }
        public DbSet<GroupEntity> Groups {get; set; }
        public DbSet<GroupUserEntity> GroupUsers {get; set; }
        public DbSet<HistoryEntity> Historys {get; set; }
        public DbSet<MessageEntity> Messages {get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ApplyEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new FriendClassifyEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new FriendEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new GroupEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new GroupUserEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new HistoryEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new MessageEntityTypeConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
