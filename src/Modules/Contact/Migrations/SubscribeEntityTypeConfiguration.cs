using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Contact.Entities;

namespace NetDream.Modules.Contact.Migrations
{
    public class SubscribeEntityTypeConfiguration : IEntityTypeConfiguration<SubscribeEntity>
    {
        public void Configure(EntityTypeBuilder<SubscribeEntity> builder)
        {
            builder.ToTable("cif_subscribe", table => table.HasComment("ÓÊÏä¶©ÔÄ"));
            builder.HasKey(i => i.Id);
            builder.Property(table => table.Id).HasColumnName("id");
            builder.HasIndex(table => table.Email).IsUnique();
            builder.Property(table => table.Email).HasColumnName("email").HasMaxLength(50);
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(30).HasDefaultValue(string.Empty).HasComment("³Æºô");
            builder.Property(table => table.Status).HasColumnName("status").HasDefaultValue(0);
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
