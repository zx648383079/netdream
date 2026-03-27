using Microsoft.EntityFrameworkCore;
using NetDream.Modules.Comment.Entities;
using NetDream.Modules.Comment.Migrations;

namespace NetDream.Modules.Comment
{
    public class CommentContext(DbContextOptions<CommentContext> options) : DbContext(options)
    {
        public DbSet<CommentEntity> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CommentEntityTypeConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
