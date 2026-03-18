using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Comment.Entities;
using NetDream.Modules.Comment.Migrations;

namespace NetDream.Modules.Comment
{
    public class CommentContext(DbContextOptions<CommentContext> options) : DbContext(options)
    {
        public DbSet<CommentEntity> Comments { get; set; }
        public DbSet<LogEntity> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CommentEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new LogEntityTypeConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
