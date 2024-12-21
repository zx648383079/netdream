using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetDream.Modules.Navigation.Entities;

namespace NetDream.Modules.Navigation.Migrations
{
    public class CollectEntityTypeConfiguration : IEntityTypeConfiguration<CollectEntity>
    {
        public void Configure(EntityTypeBuilder<CollectEntity> builder)
        {
            builder.ToTable("search_collect", table => table.HasComment("ÊÕ²ØÍøÒ³±í"));
            builder.HasKey("id");
            builder.Property(table => table.Id).HasColumnName("id");
            builder.Property(table => table.Name).HasColumnName("name").HasMaxLength(20);
            builder.Property(table => table.Link).HasColumnName("link");
            builder.Property(table => table.GroupId).HasColumnName("group_id").HasDefaultValue(0);
            builder.Property(table => table.UserId).HasColumnName("user_id").HasDefaultValue(0);
            builder.Property(table => table.Position).HasColumnName("position").HasMaxLength(1).HasDefaultValue(5);
            builder.Property(table => table.UpdatedAt).HasColumnName("updated_at");
            builder.Property(table => table.CreatedAt).HasColumnName("created_at");
        }
    }
}
