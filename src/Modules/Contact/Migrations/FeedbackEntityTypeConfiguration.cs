using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Contact.Entities;

namespace NetDream.Modules.Contact.Migrations
{
    public class FeedbackEntityTypeConfiguration : IEntityTypeConfiguration<FeedbackEntity>
    {
        public void Configure(EntityTypeBuilder<FeedbackEntity> builder)
        {
            builder.ToTable("cif_feedback", table => table.HasComment("留言"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(20);
            builder.Property(table => table.Email).HasColumnName("email").HasMaxLength(50).HasDefaultValue(string.Empty);
            builder.Property(table => table.Phone).HasColumnName("phone").HasMaxLength(30).HasDefaultValue(string.Empty);
            builder.Property(table => table.Content).HasColumnName("content").HasDefaultValue(string.Empty);
            builder.Property(table => table.Status).HasColumnName("status").HasDefaultValue(0);
            builder.Property(table => table.OpenStatus).HasColumnName("open_status").HasDefaultValue(0).HasComment("是否前台可见");
            builder.Property(table => table.UserId).HasColumnName("user_id").HasDefaultValue(0);
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
